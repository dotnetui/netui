using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Text;

namespace Net.Essentials.Vimeo.Models
{
    public class VideoFile
    {
        public string Codec { get; set; }
        [JsonProperty("created_time")] public DateTimeOffset CreatedTime { get; set; }
        public DateTimeOffset Expires { get; set; }
        public int Fps { get; set; }
        public int Height { get; set; }
        public string Link { get; set; }
        public object Log { get; set; }
        public string Md5 { get; set; }
        [JsonProperty("public_name")] public string PublicName { get; set; }
        public string Quality { get; set; }
        public string Rendition { get; set; }
        public long Size { get; set; }
        [JsonProperty("size_short")] public string SizeShort { get; set; }
        [JsonProperty("source_link")] public string SourceLink { get; set; }
        public string Type { get; set; }
        [JsonProperty("video_file_id")] public string VideoFileId { get; set; }
        public int Width { get; set; }
    }
}
