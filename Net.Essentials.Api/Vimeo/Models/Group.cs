using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Text;

namespace Net.Essentials.Vimeo.Models
{
    public class Group
    {
        [JsonProperty("created_time")] public DateTimeOffset CreatedTime { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
        public object Metadata { get; set; }
        [JsonProperty("modified_time")] public DateTimeOffset ModifiedTime { get; set; }
        public string Name { get; set; }
        public Pictures Pictures { get; set; }
        public Privacy Privacy { get; set; }
        [JsonProperty("resource_key")] public string ResourceKey { get; set; }
        public string Uri { get; set; }
        public User User { get; set; }
    }
}
