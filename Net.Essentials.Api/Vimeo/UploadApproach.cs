using System;
using System.Collections.Generic;
using System.Text;

namespace Net.Essentials.Vimeo
{
    public enum UploadApproach
    {
        Unspecified = 0,
        Tus,
        Post,
        Pull,
        LegacyResumable
    }
}
