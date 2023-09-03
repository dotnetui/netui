using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Text;

namespace Net.Essentials.Vimeo.Models
{
    public class Tag
    {
        public string Canonical { get; set; }
        public object Metadata { get; set; }
        public string Name { get; set; }
        [JsonProperty("resource_key")] public string ResourceKey { get; set; }
        public string Uri { get; set; }
    }
}
