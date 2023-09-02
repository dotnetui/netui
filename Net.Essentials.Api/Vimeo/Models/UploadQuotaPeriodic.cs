using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Text;

namespace Net.Essentials.Vimeo.Models
{
    public class UploadQuotaPeriodic
    {
        public UploadQuotaPeriod Period { get; set; }
        [JsonProperty("unit")] public string RawUnit { get; set; }
        public long Free { get; set; }
        public long Max { get; set; }
        public long Used { get; set; }
        [JsonProperty("reset_date")] public DateTimeOffset ResetDate { get; set; }

        [JsonIgnore] public UploadQuotaUnit Unit => UploadQuotaExtensions.ToUploadQuotaUnit(RawUnit);
    }
}
