using Net.Essentials.Vimeo.JsonConverters;
using Net.Essentials.Vimeo.Models;

using Newtonsoft.Json;

using RestSharp;

using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Net.Essentials.Vimeo
{
    public partial class VimeoClient : ApiClient
    {
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
            if (response.IsOk())
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

        public async Task<User> GetUserByIdAsync(string id)
        {
            return await RequestAsync<User>($"users/{id}");
        }

        public async Task<User> GetMeAsync()
        {
            var result = await RequestAsync<User>("me");
            return result;
        }
    }
}
