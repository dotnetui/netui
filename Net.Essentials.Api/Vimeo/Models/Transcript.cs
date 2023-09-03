using Net.Essentials.Converters;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Text;

namespace Net.Essentials.Vimeo.Models
{
    public class Transcript
    {
        [JsonProperty("status")] public string RawStatus { get; set; }
        [JsonIgnore] public TranscriptStatus Status => StringEnumConverterRepository.Default.GetValue<TranscriptStatus>(RawStatus);
    }
}
