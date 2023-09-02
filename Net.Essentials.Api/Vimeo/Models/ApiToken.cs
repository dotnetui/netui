using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Text;

namespace Net.Essentials.Vimeo.Models
{
    public class ApiToken
    {
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.Now;
        public DateTimeOffset? ExpiresAt => ExpiresIn.HasValue ? CreatedAt.AddSeconds(ExpiresIn.Value) : (DateTimeOffset?)null;
        public bool IsExpired => ExpiresAt.HasValue ? ExpiresAt.Value < DateTimeOffset.Now : false;

        [JsonProperty("access_token")] public string AccessToken { get; set; }
        [JsonProperty("token_type")] public string TokenType { get; set; }
        [JsonProperty("expires_in")] public int? ExpiresIn { get; set; }
        [JsonProperty("scope")] public string Scope { get; set; }
        [JsonProperty("app")] public AppProfile App { get; set; }
        [JsonProperty("user")] public User User { get; set; }
    }
}
