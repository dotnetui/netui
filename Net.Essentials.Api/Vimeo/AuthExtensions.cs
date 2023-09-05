using Net.Essentials.Vimeo.Models;
using Newtonsoft.Json.Linq;
using RestSharp;

using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Net.Essentials.Vimeo
{
    public static class AuthExtensions
    {
        /// <summary>
        /// Request a public access token with your Client Id and Client Secret
        /// </summary>
        /// <returns>A public access token</returns>
        /// <exception cref="VimeoException"></exception>
        public static async Task<ApiToken> AuthorizeClientAsync(this VimeoClient client)
        {
            client.Token = default;
            var response = await client.RequestAsync("/oauth/authorize/client", Method.Post, new
            {
                grant_type = "client_credentials",
                scope = "public"
            });
            if (response.IsSuccessful)
            {
                return client.Token = client.DeserializeResponse<ApiToken>(response);
            }
            else
            {
                throw new VimeoException(response.Content);
            }
        }

        public static string GetAuthorizationUrl(this VimeoClient client, AuthResponseType type, string state, IEnumerable<string> scopes, string clientId = default, string redirectUri = default)
        {
            clientId = clientId ?? client.App.ClientId;
            redirectUri = redirectUri ?? client.App.CallbackUrl;
            var scope = string.Join(" ", scopes);
            var sType = type.ToString().ToLower();
            return $"https://api.vimeo.com/oauth/authorize?response_type={sType}&client_id={clientId}&redirect_uri={redirectUri}&state={state}&scope={scope}";
        }

        public static string GetAuthorizationUrl(this VimeoClient client, AuthResponseType type, IEnumerable<string> scopes, string clientId = default, string redirectUri = default)
        {
            var state = IdExtensions.GenerateId();
            return client.GetAuthorizationUrl(type, state, scopes, clientId, redirectUri);
        }

        public static string GetAuthorizationUrl(this VimeoClient client, AuthResponseType type, string state, Scopes scopes, string clientId = default, string redirectUri = default)
        {
            return client.GetAuthorizationUrl(type, state, ScopeExtensions.ToStringArray(scopes), clientId, redirectUri);
        }

        public static string GetAuthorizationUrl(this VimeoClient client, AuthResponseType type, Scopes scopes, string clientId = default, string redirectUri = default)
        {
            var state = IdExtensions.GenerateId();
            return client.GetAuthorizationUrl(type, state, scopes, clientId, redirectUri);
        }
        
        public static async Task<ApiToken> AuthorizeAsync(this VimeoClient client, AuthorizationParameters parameters)
        {
            if (!string.IsNullOrWhiteSpace(parameters.AccessToken))
            {
                return client.Token = new ApiToken
                {
                    AccessToken = parameters.AccessToken,
                    Scope = parameters.Scope?.Replace("%20", " "),
                    TokenType = parameters.TokenType,
                    ExpiresIn = parameters.ExpiresIn,
                    CreatedAt = parameters.CreatedAt
                };
            }

            return client.Token = await client.ExchangeAuthorizationCodeForAccessTokenAsync(parameters.Code, parameters.RedirectUri);
        }


        public static async Task<ApiToken> AuthorizeAsync(this VimeoClient client, string callbackUrl)
        {
            var parameters = AuthorizationParameters.FromUrl(callbackUrl);
            return await client.AuthorizeAsync(parameters);
        }

        public static async Task<DeviceCodeGrant> GetDeviceCodeGrantAsync(this VimeoClient client, IEnumerable<string> scopes)
        {
            return await client.RequestAsync<DeviceCodeGrant>("/oauth/device", Method.Post, new
            {
                grant_type = "device_grant",
                scope = string.Join(" ", scopes)
            });
        }

        public static async Task<DeviceCodeGrant> GetDeviceCodeGrantAsync(this VimeoClient client, Scopes scopes)
        {
            return await client.GetDeviceCodeGrantAsync(ScopeExtensions.ToStringArray(scopes));
        }

        public static async Task<DeviceAuthorizeResponse> PollAuthorizeLinkAsync(this VimeoClient client, DeviceCodeGrant grant)
        {
            var restClient = new RestClient();
            var request = client.CreateRequest(grant.AuthorizeLink, Method.Post);
            request.AddJsonBody(new
            {
                user_code = grant.UserCode,
                device_code = grant.DeviceCode
            });
            var response = await restClient.ExecuteAsync(request);
            var result = new DeviceAuthorizeResponse
            {
                Response = response
            };
            if (response.IsSuccessful)
            {
                try
                {
                    result.Token = client.DeserializeResponse<ApiToken>(response);
                    if (result.Token != default) client.Token = result.Token;
                }
                catch { }
            }
            return result;
        }

        public static async Task<bool> DeleteTokensAsync(this VimeoClient client)
        {
            return (await client.RequestAsync("/tokens", Method.Delete)).StatusCode == HttpStatusCode.NoContent;
        }

        public static async Task<bool> VerifyOAuth2AccessTokenAsync(this VimeoClient client)
        {
            return VimeoClient.IsOk(await client.RequestAsync("/oauth/verify"));
        }


        public static async Task<ApiToken> ConvertOAuth1AccessTokenToOAuth2Async(this VimeoClient client, string token, string tokenSecret)
        {
            return await client.RequestAsync<ApiToken>("/oauth/authorize/vimeo_oauth1", Method.Post, new
            {
                grant_type = "vimeo_oauth1",
                token,
                token_secret = tokenSecret
            });
        }

        public static async Task<ApiToken> ExchangeAuthorizationCodeForAccessTokenAsync(this VimeoClient client, string code, string redirectUri)
        {
            var response = await client.RequestAsync("/oauth/access_token", Method.Post, new
            {
                grant_type = "authorization_code",
                redirect_uri = redirectUri,
                code
            });

            if (response.IsSuccessful)
            {
                return client.DeserializeResponse<ApiToken>(response);
            }
            else
            {
                throw new VimeoException(response.Content);
            }
        }
    }
}
