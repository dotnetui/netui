using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Text;

namespace Net.Essentials.Vimeo.Models
{
    public class Project
    {
        [JsonProperty("access_grant")] public object AccessGrant { get; set; }
        [JsonProperty("created_time")] public DateTimeOffset CreatedTime { get; set; }
        [JsonProperty("creator_uri")] public string CreatorUri { get; set; }
        [JsonProperty("has_subfolder")] public bool HasSubfolder { get; set; }
        [JsonProperty("is_pinned")] public bool IsPinned { get; set; }
        [JsonProperty("is_private_to_user")] public bool IsPrivateToUser { get; set; }
        [JsonProperty("last_user_action_event_date")] public DateTimeOffset LastUserActionEventDate { get; set; }
        public string Link { get; set; }
        public object Metadata { get; set; }
        [JsonProperty("modified_time")] public DateTimeOffset ModifiedTime { get; set; }
        public string Name { get; set; }
        [JsonProperty("pinned_on")] public DateTimeOffset PinnedOn { get; set; }
        public Privacy Privacy { get; set; }
        [JsonProperty("resource_key")] public string ResourceKey { get; set; }
        public object Settings { get; set; }
        public string Uri { get; set; }
        public User User { get; set; }
    }
}
