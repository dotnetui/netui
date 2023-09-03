using Net.Essentials.Converters;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Text;

namespace Net.Essentials.Vimeo.Models
{
    public class User
    {
        public string Uri { get; set; }
        public string Name { get; set; }
        public string Link { get; set; }
        public Capabilities Capabilities { get; set; }
        public string Location { get; set; }
        public string Gender { get; set; }
        public string Bio { get; set; }
        [JsonProperty("short_bio")] public string ShortBio { get; set; }
        [JsonProperty("created_time")] public DateTimeOffset CreatedTime { get; set; }
        public List<Website> Websites { get; set; }
        public Metadata Metadata { get; set; }
        [JsonProperty("location_details")] public LocationDetails LocationDetails { get; set; }
        public Skill[] Skills { get; set; }
        [JsonProperty("available_for_hire")] public bool AvailableForHire { get; set; }
        [JsonProperty("can_work_remotely")] public bool CanWorkRemotely { get; set; }
        public UserPreferences Preferences { get; set; }
        [JsonProperty("content_filter")] public string[] RawContentFilter { get; set; }
        [JsonProperty("resource_key")] public string ResourceKey { get; set; }
        [JsonProperty("account")] public string RawAccount { get; set; }
        [JsonProperty("has_invalid_email")] public bool HasInvalidEmail { get; set; }
        [JsonProperty("is_expert")] public bool IsExpert { get; set; }
        [JsonProperty("upload_quota")] public UploadQuota UploadQuota { get; set; }
        public Pictures Pictures { get; set; }

        [JsonIgnore] public AccountType Account => StringEnumConverterRepository.Default.GetValue<AccountType>(RawAccount);
        [JsonIgnore] public ContentFilters ContentFilters => ContentFiltersExtensions.ToContentFilters(RawContentFilter);
    }
}
