using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Text;

namespace Net.Essentials.Vimeo.Models
{
    public class Picture
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public string Link { get; set; }
        [JsonProperty("link_with_play_button")] public string LinkWithPlayButton { get; set; }
    }
}
