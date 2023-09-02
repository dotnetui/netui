using System;
using System.Collections.Generic;
using System.Text;

namespace Net.Essentials.Vimeo.Models
{
    public class UserPreferences
    {
        public UserVideoPreferences Videos { get; set; }
        public object[] webinar_registrant_lower_watermark_banner_dismissed { get; set; }
    }
}
