using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Net.Essentials.Vimeo
{
    public class AuthorizationParameters
    {
        public string RedirectUri { get; set; }
        public string State { get; set; }
        public string Code { get; set; }
        public DateTimeOffset CreatedAt { get; } = DateTimeOffset.Now;
        public string TokenType { get; set; }
        public string AccessToken { get; set; }
        public string Scope { get; set; }
        public int? ExpiresIn { get; set; }

        public AuthorizationParameters(string redirectUri, string state, string code)
        {
            RedirectUri = redirectUri;
            State = state;
            Code = code;
        }
        
        public AuthorizationParameters(string redirectUri, string code)
        {
            RedirectUri = redirectUri;
            Code = code;
        }

        public AuthorizationParameters()
        {
        }

        public override string ToString()
        {
            return $"redirect_uri={RedirectUri}&state={State}&code={Code}";
        }

        public static AuthorizationParameters FromUrl(string url)
        {
            var result = new AuthorizationParameters();

            var hashSplit = url.Split('#');
            var split = hashSplit[0].Split('?');
            var parameters = split[1]
                .Split('&')
                .Select(x => x.Split('='))
                .Where(x => x.Length == 2)
                .ToDictionary(x => x[0], x => x[1]);
            result.RedirectUri = split[0];
            parameters.TryGetValue("state", out var state);
            parameters.TryGetValue("code", out var code);

            result.Code = code;

            if (hashSplit.Length > 1)
            {
                var hashParams = hashSplit[1]
                    .Split('&')
                    .Select(x => x.Split('='))
                    .Where(x => x.Length == 2)
                    .ToDictionary(x => x[0], x => x[1]);
                result.AccessToken = hashParams["access_token"];
                result.TokenType = hashParams["token_type"];
                result.Scope = hashParams["scope"];
                result.State = state ?? hashParams["state"];
                result.ExpiresIn = int.Parse(hashParams["expires_in"]);
            }

            return result;
        }
    }
}
