using System;
using System.Collections.Generic;
using System.Text;

namespace Net.Essentials.Vimeo.Models
{
    public static class UploadQuotaExtensions
    {
        public static UploadQuotaType ToUploadQuotaType(string s)
        {
            if (Enum.TryParse<UploadQuotaType>(s, true, out var r))
                return r;
            return UploadQuotaType.Unknown;
        }

        public static UploadQuotaUnit ToUploadQuotaUnit(string s)
        {
            if (s == "video_count") return UploadQuotaUnit.VideoCount;
            if (s == "video_size") return UploadQuotaUnit.VideoSize;
            return UploadQuotaUnit.Unknown;
        }

        public static UploadQuotaPeriod ToUploadQuotaPeriod(string s)
        {
            if (Enum.TryParse<UploadQuotaPeriod>(s, true, out var r))
                return r;
            return UploadQuotaPeriod.Unknown;
        }
    }
}
