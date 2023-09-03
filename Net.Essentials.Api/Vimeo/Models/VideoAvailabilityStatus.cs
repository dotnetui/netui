using System;
using System.Collections.Generic;
using System.Text;

namespace Net.Essentials.Vimeo.Models
{
    public enum VideoAvailabilityStatus
    {
        Unspecified,
        QuotaExceeded,
        TotalCapExceeded,
        TranscodeStarting,
        Transcoding,
        TranscodingError,
        Unavailable,
        Uploading,
        UploadingError
    }
}
