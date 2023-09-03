using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Text;

namespace Net.Essentials.Vimeo.Models
{
    public class UserVideoPreferences
    {
        [JsonProperty("rating")] public string[] RawRating { get; set; }
        public string Password { get; set; }
        public Privacy Privacy { get; set; }

        [JsonIgnore] public ContentFilters Rating => ContentFiltersExtensions.ToContentFilters(RawRating);
    }
}
