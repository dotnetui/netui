using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Text;

namespace Net.Essentials.Vimeo.Models
{
    public class Connections
    {
        public ConnectionMetadata Albums { get; set; }
        public ConnectionMetadata Appearances { get; set; }
        public ConnectionMetadata Categories { get; set; }
        public ConnectionMetadata Channels { get; set; }
        public ConnectionMetadata Feed { get; set; }
        public ConnectionMetadata Followers { get; set; }
        public ConnectionMetadata Following { get; set; }
        public ConnectionMetadata Groups { get; set; }
        public ConnectionMetadata Likes { get; set; }
        public ConnectionMetadata Membership { get; set; }
        [JsonProperty("moderated_channels")] public ConnectionMetadata ModeratedChannels { get; set; }
        public ConnectionMetadata Portfolios { get; set; }
        public ConnectionMetadata Videos { get; set; }
        public ConnectionMetadata Watchlater { get; set; }
        public ConnectionMetadata Shared { get; set; }
        public ConnectionMetadata Pictures { get; set; }
        [JsonProperty("watched_videos")] public ConnectionMetadata WatchedVideos { get; set; }
        [JsonProperty("folders_root")] public ConnectionMetadata FoldersRoot { get; set; }
        public ConnectionMetadata Folders { get; set; }
        public ConnectionMetadata Teams { get; set; }
        public ConnectionMetadata Block { get; set; }
    }
}
