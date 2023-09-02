using System;
using System.Collections.Generic;
using System.Text;

namespace Net.Essentials.Vimeo.Models
{
    public static class EmbedPrivacyExtensions
    {
        public static EmbedPrivacy ToEmbedPrivacy(string embedPrivacyString)
        {
            if (Enum.TryParse<EmbedPrivacy>(embedPrivacyString, true, out var result))
                return result;
            return EmbedPrivacy.Unknown;
        }

        public static string ToEmbedPrivacyString(this EmbedPrivacy embedPrivacy)
        {
            if (embedPrivacy == EmbedPrivacy.Unknown) return null;
            return embedPrivacy.ToString().ToLower();
        }
    }
}
