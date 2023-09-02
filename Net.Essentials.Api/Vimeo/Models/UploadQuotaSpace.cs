using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Text;

namespace Net.Essentials.Vimeo.Models
{
    public class UploadQuotaSpace
    {
        public long Free { get; set; }
        public long Max { get; set; }
        public long Used { get; set; }
        [JsonProperty("showing")] public string RawShowing { get; set; }
        [JsonProperty("unit")] public string RawUnit { get; set; }

        [JsonIgnore] public UploadQuotaType Showing => UploadQuotaExtensions.ToUploadQuotaType(RawShowing);
        [JsonIgnore] public UploadQuotaUnit Unit => UploadQuotaExtensions.ToUploadQuotaUnit(RawUnit);
    }
}
