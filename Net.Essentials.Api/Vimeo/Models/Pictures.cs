using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Text;

namespace Net.Essentials.Vimeo.Models
{
    public class Pictures
    {
        public string Uri { get; set; }
        public bool Active { get; set; }
        [JsonProperty("type")] public string RawType { get; set; }
        [JsonProperty("base_link")] public string BaseLink { get; set; }
        public string Link { get; set; }
        public List<Picture> Sizes { get; set; }
        [JsonProperty("resource_key")] public string ResourceKey { get; set; }
        [JsonProperty("default_picture")] public bool DefaultPicture { get; set; }

        public PictureType Type => PictureTypeExtensions.ToPictureType(RawType);
    }
}
