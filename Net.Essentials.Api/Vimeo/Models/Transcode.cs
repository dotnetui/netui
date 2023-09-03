using Net.Essentials.Converters;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Text;

namespace Net.Essentials.Vimeo.Models
{
    public class Transcode
    {
        [JsonProperty("status")] public string RawStatus { get; set; }
        [JsonIgnore] public TranscodeStatus Status => StringEnumConverterRepository.Default.GetValue<TranscodeStatus>(RawStatus);
    }
}
