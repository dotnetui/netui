using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Text;

namespace Net.Essentials.Vimeo.Models
{
    public class ConnectionMetadata
    {
        [JsonProperty("uri")] public string Uri { get; set; }
        public string[] Options { get; set; }
        public int Total { get; set; }

        [JsonConstructor]
        public ConnectionMetadata(string uri)
        {
            Uri = uri;
        }
    }
}
