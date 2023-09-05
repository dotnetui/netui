using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Text;

namespace Net.Essentials.Vimeo.Models
{
    public class Collection<T>
    {
        [JsonProperty("direction")] public string RawDirection { get; set; }
        [JsonIgnore] public SortDirection Direction => Enum.TryParse<SortDirection>(RawDirection, true, out var direction) ? direction : default;
        public int Page { get; set; }
        public int Total { get; set; }
        [JsonProperty("per_page")] public int PerPage { get; set; }
        public string Sort { get; set; }
        public List<T> Data { get; set; }
        public Paging Paging { get; set; }
    }
}
