using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Text;

namespace Net.Essentials.Vimeo
{
    public class DeviceCodeGrant
    {
        [JsonProperty("device_code")] public string DeviceCode { get; set; }
        [JsonProperty("user_code")] public string UserCode { get; set; }
        [JsonProperty("authorize_link")] public string AuthorizeLink { get; set; }
        [JsonProperty("activate_link")] public string ActivateLink { get; set; }
        [JsonProperty("expires_in")] public int ExpiresIn { get; set; }
        [JsonProperty("interval")] public int Interval { get; set; }
    }
}
