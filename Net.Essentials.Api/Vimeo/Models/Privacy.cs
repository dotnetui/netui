using Net.Essentials.Converters;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Text;

namespace Net.Essentials.Vimeo.Models
{
    public class Privacy
    {
        [JsonProperty("view")] public string RawView { get; set; }
        [JsonProperty("comments")] public string RawComments { get; set; }
        [JsonProperty("embed")] public string RawEmbed { get; set; }
        public bool Download { get; set; }
        public bool Add { get; set; }
        [JsonProperty("allow_share_link")] public bool AllowShareLink { get; set; }
        public string Password { get; set; }

        [JsonIgnore] public CommentsPrivacy Comments => StringEnumConverterRepository.Default.GetValue<CommentsPrivacy>(RawComments);
        [JsonIgnore] public EmbedPrivacy Embed => StringEnumConverterRepository.Default.GetValue<EmbedPrivacy>(RawEmbed);
        [JsonIgnore] public ViewPrivacy View => StringEnumConverterRepository.Default.GetValue<ViewPrivacy>(RawView);
    }
}
