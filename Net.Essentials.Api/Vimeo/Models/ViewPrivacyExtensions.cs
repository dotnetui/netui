using System;
using System.Collections.Generic;
using System.Text;

namespace Net.Essentials.Vimeo.Models
{
    public static class ViewPrivacyExtensions
    {
        public static ViewPrivacy ToViewPrivacy(string viewPrivacyString)
        {
            if (Enum.TryParse<ViewPrivacy>(viewPrivacyString, true, out var result))
                return result;
            return ViewPrivacy.Unknown;
        }

        public static string ToViewPrivacyString(this ViewPrivacy viewPrivacy)
        {
            if (viewPrivacy == ViewPrivacy.Unknown) return null;
            return viewPrivacy.ToString().ToLower();
        }
    }
}
