using System;
using System.Collections.Generic;
using System.Text;

namespace Net.Essentials.Vimeo.Models
{
    public static class ContentFiltersExtensions
    {
        public static ContentFilters ToContentFilters(IEnumerable<string> filters)
        {
            var result = ContentFilters.None;
            if (filters != null) foreach (var filter in filters)
                {
                    if (filter == "drugs") result |= ContentFilters.Drugs;
                    else if (filter == "language") result |= ContentFilters.Language;
                    else if (filter == "nudity") result |= ContentFilters.Nudity;
                    else if (filter == "safe") result |= ContentFilters.Safe;
                    else if (filter == "unrated") result |= ContentFilters.Unrated;
                    else if (filter == "violence") result |= ContentFilters.Violence;
                    else result |= ContentFilters.Undefined;
                }
            return result;
        }

        public static List<string> ToStringArray(this ContentFilters filters)
        {
            var results = new List<string>();
            if (filters.HasFlag(ContentFilters.Drugs)) results.Add("drugs");
            if (filters.HasFlag(ContentFilters.Language)) results.Add("language");
            if (filters.HasFlag(ContentFilters.Nudity)) results.Add("nudity");
            if (filters.HasFlag(ContentFilters.Safe)) results.Add("safe");
            if (filters.HasFlag(ContentFilters.Unrated)) results.Add("unrated");
            if (filters.HasFlag(ContentFilters.Violence)) results.Add("violence");
            return results;
        }
    }
}
