using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Text;

namespace Net.Essentials.Vimeo.Models
{
    public class UploadQuota
    {
        public UploadQuotaSpace Space { get; set; }
        public UploadQuotaPeriodic Periodic { get; set; }
        public UploadQuotaSpace Lifetime { get; set; }
    }
}
