using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Text;

namespace Net.Essentials.Vimeo.Models
{
    public class Category
    {
        public Pictures Icon { get; set; }
        [JsonProperty("is_deprecated")] public bool IsDeprecated { get; set; }
        [JsonProperty("last_video_featured_time")] public DateTimeOffset LastVideoFeaturedTime { get; set; }
        public string Link { get; set; }
        public object Metadata { get; set; }
        public string Name { get; set; }
        public Category Parent { get; set; }
        public Pictures Pictures { get; set; }
        [JsonProperty("resource_key")] public string ResourceKey { get; set; }
        public Category[] Subcategories { get; set; }
        [JsonProperty("top_level")] public bool TopLevel { get; set; }
        public string Uri { get; set; }
    }
}
