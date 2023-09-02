using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Text;

namespace Net.Essentials.Vimeo.Models
{
    public class PrivacyPreferences
    {
        [JsonProperty("view")] public string RawView { get; set; }
        [JsonProperty("comments")] public string RawComments { get; set; }
        [JsonProperty("embed")] public string RawEmbed { get; set; }
        public bool Download { get; set; }
        public bool Add { get; set; }
        [JsonProperty("allow_share_link")] public bool AllowShareLink { get; set; }
        public string Password { get; set; }

        [JsonIgnore] public CommentsPrivacy Comments => CommentsPrivacyExtensions.ToCommentsPrivacy(RawComments);
        [JsonIgnore] public EmbedPrivacy Embed => EmbedPrivacyExtensions.ToEmbedPrivacy(RawEmbed);
        [JsonIgnore] public ViewPrivacy View => ViewPrivacyExtensions.ToViewPrivacy(RawView);
    }
}
