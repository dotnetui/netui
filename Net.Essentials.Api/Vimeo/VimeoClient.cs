using Net.Essentials.Components;
using Net.Essentials.Vimeo.JsonConverters;
using Net.Essentials.Vimeo.Models;

using Newtonsoft.Json;

using RestSharp;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Net.Essentials.Vimeo
{
    public partial class VimeoClient : ApiClient
    {
        static bool IsOk(RestResponse response) => response != null &&
            response.StatusCode == HttpStatusCode.OK ||
            response.StatusCode == HttpStatusCode.Created ||
            response.StatusCode == HttpStatusCode.PartialContent ||
            response.StatusCode == HttpStatusCode.Accepted;

        public VimeoApp App { get; set; }
        public ApiToken Token { get; set; }

        public bool IsAuthorized => Token != null && !Token.IsExpired;

        public VimeoClient(VimeoApp app) : this()
        {
            App = app;
        }

        public VimeoClient(string accessToken) : this()
        {
            Token = new ApiToken
            {
                AccessToken = accessToken
            };
        }

        public VimeoClient()
        {
            BaseUrl = "https://api.vimeo.com";
        }

        protected override RestRequest CreateRequest(string path, Method method)
        {
            var request = base.CreateRequest(path, method);
            if (Token?.AccessToken != null)
                request.AddHeader("Authorization", $"Bearer {Token.AccessToken}");
            else if (!string.IsNullOrWhiteSpace(App?.ClientSecret) && !string.IsNullOrWhiteSpace(App?.ClientId))
                request.AddHeader("Authorization", $"basic {Base64Encode(App.ClientId, App.ClientSecret)}");
            request.AddHeader("Accept", "application/vnd.vimeo.*+json;version=3.4");
            return request;
        }

        string Base64Encode(string clientId, string clientSecret)
        {
            var bytes = Encoding.UTF8.GetBytes($"{clientId}:{clientSecret}");
            return Convert.ToBase64String(bytes);
        }

        /// <summary>
        /// Request a public access token with your Client Id and Client Secret
        /// </summary>
        /// <returns>A public access token</returns>
        /// <exception cref="VimeoException"></exception>
        public async Task<ApiToken> AuthorizeClientAsync()
        {
            Token = default;
            var response = await RequestAsync("/oauth/authorize/client", Method.Post, new
            {
                grant_type = "client_credentials",
                scope = "public"
            });
            if (response.IsSuccessful)
            {
                return Token = DeserializeResponse<ApiToken>(response);
            }
            else
            {
                throw new VimeoException(response.Content);
            }
        }

        public string GetAuthorizationUrl(AuthResponseType type, string state, IEnumerable<string> scopes, string clientId = default, string redirectUri = default)
        {
            clientId = clientId ?? App.ClientId;
            redirectUri = redirectUri ?? App.CallbackUrl;
            var scope = string.Join(" ", scopes);
            var sType = type.ToString().ToLower();
            return $"https://api.vimeo.com/oauth/authorize?response_type={sType}&client_id={clientId}&redirect_uri={redirectUri}&state={state}&scope={scope}";
        }
        
        public string GetAuthorizationUrl(AuthResponseType type, IEnumerable<string> scopes, string clientId = default, string redirectUri = default)
        {
            var state = IdExtensions.GenerateId();
            return GetAuthorizationUrl(type, state, scopes, clientId, redirectUri);
        }
        
        public string GetAuthorizationUrl(AuthResponseType type, Scopes scopes, string clientId = default, string redirectUri = default)
        {
            var state = IdExtensions.GenerateId();
            return GetAuthorizationUrl(type, state, scopes, clientId, redirectUri);
        }
        
        public string GetAuthorizationUrl(AuthResponseType type, string state, Scopes scopes, string clientId = default, string redirectUri = default)
        {
            return GetAuthorizationUrl(type, state, ScopeExtensions.ToStringArray(scopes), clientId, redirectUri);
        }

        public async Task<ApiToken> AuthorizeAsync(string callbackUrl)
        {
            var parameters = AuthorizationParameters.FromUrl(callbackUrl);
            return await AuthorizeAsync(parameters);
        }

        public async Task<ApiToken> AuthorizeAsync(AuthorizationParameters parameters)
        {
            if (!string.IsNullOrWhiteSpace(parameters.AccessToken))
            {
                return Token = new ApiToken
                {
                    AccessToken = parameters.AccessToken,
                    Scope = parameters.Scope?.Replace("%20", " "),
                    TokenType = parameters.TokenType,
                    ExpiresIn = parameters.ExpiresIn,
                    CreatedAt = parameters.CreatedAt
                };
            }

            Token = default;
            var response = await RequestAsync("/oauth/access_token", Method.Post, new
            {
                grant_type = "authorization_code",
                redirect_uri = parameters.RedirectUri,
                code = parameters.Code
            });
            if (response.IsSuccessful)
            {
                return Token = DeserializeResponse<ApiToken>(response);
            }
            else
            {
                throw new VimeoException(response.Content);
            }
        }

        public async Task<DeviceCodeGrant> GetDeviceCodeGrantAsync(Scopes scopes)
        {
            return await GetDeviceCodeGrantAsync(ScopeExtensions.ToStringArray(scopes));
        }

        public async Task<DeviceCodeGrant> GetDeviceCodeGrantAsync(IEnumerable<string> scopes)
        {
            return await RequestAsync<DeviceCodeGrant>("/oauth/device", Method.Post, new
            {
                grant_type = "device_grant",
                scope = string.Join(" ", scopes)
            });
        }

        public async Task<DeviceAuthorizeResponse> PollAuthorizeLinkAsync(DeviceCodeGrant grant)
        {
            var client = new RestClient();
            var request = CreateRequest(grant.AuthorizeLink, Method.Post);
            request.AddJsonBody(new
            {
                user_code = grant.UserCode,
                device_code = grant.DeviceCode
            });
            var response = await client.ExecuteAsync(request);
            var result = new DeviceAuthorizeResponse
            {
                Response = response
            };
            if (response.IsSuccessful)
            {
                try
                {
                    result.Token = DeserializeResponse<ApiToken>(response);
                    if (result.Token != default) Token = result.Token;
                }
                catch { }
            }
            return result;
        }

        static readonly JsonSerializerSettings DeserializeSettings = new JsonSerializerSettings
        {
            Error = (s, e) =>
            {
                Console.WriteLine(e.ErrorContext.Error);
                e.ErrorContext.Handled = true;
            },
            Converters = new List<JsonConverter>
            {
                new ConnectionMetadataJsonConverter(),
                new PicturesJsonConverter(),
                new PrivacyPreferencesJsonConverter()
            }
        };

        public override T DeserializeResponse<T>(RestResponse response, Action onfail = null)
        {
            if (response == null) return default;
            if (IsOk(response))
            {
                try
                {
                    return JsonConvert.DeserializeObject<T>(response.Content ?? "", DeserializeSettings);
                }
                catch (Exception ex)
                {
                    Log($"RequestAsync deserialization failed: {ex}");
                }
            }

            if (response != null)
                throw new VimeoException(response.Content);

            return default;
        }

        public async Task<RestResponse> GetOpenApiSpecsAsync()
        {
            return await RequestAsync("/");
        }

        public async Task<User> GetUserByIdAsync(string id)
        {
            return await RequestAsync<User>($"users/{id}");
        }

        public async Task<User> GetMeAsync()
        {
            var result = await RequestAsync<User>("me");
            return result;
        }

        public async Task<UploadTicket> CreateVideoAsync(long size, UploadApproach approach = UploadApproach.Tus, string url = default, Dictionary<string, object> metadata = default)
        {
            var upload = new Dictionary<string, object>
            {
                { "approach", approach.ToString().ToLower() },
                { "size", size.ToString() }
            };
            if (url != null)
                upload.Add(approach == UploadApproach.Post ? "redirect_url" : "link", WebUtility.UrlEncode(url));

            Dictionary<string, object> payload = metadata == null ? new Dictionary<string, object>() : new Dictionary<string, object>(metadata);
            payload["upload"] = upload;

            var result = await RequestAsync<UploadTicket>("/me/videos", HttpMethod.Post, payload);
            return result;
        }

        public async Task<bool> UploadTusChunkAsync(string uploadLink, string path, long offset, long take = -1)
        {
            var tcpUploader = new TcpUploader();
            tcpUploader.OnProgress += (object s, EventArgs e) => Console.WriteLine($"Progress: {tcpUploader.Position}/{tcpUploader.Total} {tcpUploader.IsActive}");
            tcpUploader.Headers.Add("Tus-Resumable", "1.0.0");
            tcpUploader.Headers.Add("Upload-Offset", offset.ToString());
            tcpUploader.ContentType = "application/offset+octet-stream";
            tcpUploader.Method = "PATCH";
            tcpUploader.Url = uploadLink;
            tcpUploader.StreamBytes = await LoadFileChunkAsync(path, offset, take);
            var result = await tcpUploader.ExecuteAsync();
            return result != null;
        }

        async Task<byte[]> LoadFileChunkAsync(string path, long offset, long take = -1)
        {
            byte[] buffer = null;
            await Task.Run(() =>
            {
                using (var fi = File.OpenRead(path))
                {
                    fi.Seek(offset, SeekOrigin.Begin);
                    if (take < 0 || fi.Length - offset < take) take = fi.Length - offset;
                    if (take > 0)
                    {
                        buffer = new byte[take];
                        fi.Read(buffer, 0, buffer.Length);
                    }
                }
            });
            return buffer;
        }
    }
}
