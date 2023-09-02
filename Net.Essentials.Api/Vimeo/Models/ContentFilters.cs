using System;
using System.Collections.Generic;
using System.Text;

namespace Net.Essentials.Vimeo.Models
{
    [Flags]
    public enum ContentFilters
    {
        None = 0,
        Undefined = 1,
        Drugs = 2,
        Language = 4,
        Nudity = 8,
        Safe = 16,
        Unrated = 32,
        Violence = 64
    }
}
