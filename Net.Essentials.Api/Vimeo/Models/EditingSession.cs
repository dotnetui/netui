using Net.Essentials.Converters;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Text;

namespace Net.Essentials.Vimeo.Models
{
    public class EditingSession
    {
        [JsonProperty("has_watermark")] public bool HasWatermark { get; set; }
        [JsonProperty("is_edited_by_tve")] public bool IsEditedByTve { get; set; }
        [JsonProperty("is_max_resolution")] public bool IsMaxResolution { get; set; }
        [JsonProperty("is_music_licensed") ] public bool IsMusicLicensed { get; set; }
        [JsonProperty("is_rated")] public bool IsRated { get; set; }
        [JsonProperty("min_tier_for_movie")] public string MinTierForMovie { get; set; }
        [JsonProperty("result_video_hash")] public string ResultVideoHash { get; set; }
        [JsonProperty("status")] public string RawStatus { get; set; }
        [JsonIgnore] public EditingSessionStatus Status => StringEnumConverterRepository.Default.GetValue<EditingSessionStatus>(RawStatus);
        public string Vsid { get; set; }
    }
}
