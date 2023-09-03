using Net.Essentials.Vimeo.JsonConverters;

using System;
using System.Collections.Generic;
using System.Text;

namespace Net.Essentials.Vimeo.Models
{
    public enum CommentsPrivacy
    {
        Unknown,
        Anybody,
        Contacts,
        Nobody
    }

    public class CommentsPrivacyEnumBinding : StringEnumBinding<CommentsPrivacy> {
        public static CommentsPrivacyEnumBinding Instance { get; } = new CommentsPrivacyEnumBinding();
    }
}
