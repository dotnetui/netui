using System;
using System.Collections.Generic;
using System.Text;

namespace Net.Essentials.Vimeo.Models
{
    public static class CommentsPrivacyExtensions
    {
        public static CommentsPrivacy ToCommentsPrivacy(string commentsPrivacyString)
        {
            if (Enum.TryParse<CommentsPrivacy>(commentsPrivacyString, true, out var result))
                return result;
            return CommentsPrivacy.Unknown;
        }

        public static string ToCommentsPrivacyString(this CommentsPrivacy commentsPrivacy)
        {
            if (commentsPrivacy == CommentsPrivacy.Unknown) return null;
            return commentsPrivacy.ToString().ToLower();
        }
    }
}
