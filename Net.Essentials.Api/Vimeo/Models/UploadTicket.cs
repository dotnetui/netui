using Net.Essentials.Converters;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Text;

namespace Net.Essentials.Vimeo.Models
{
    public class UploadTicket
    {
        [JsonProperty("approach")] public string RawApproach { get; set; }
        [JsonIgnore] public UploadApproach Approach => StringEnumConverterRepository.Default.GetValue<UploadApproach>(RawApproach);
        public string Form { get; set; }
        [JsonProperty("gcs_uid")] public string GcsUid { get; set; }
        public string Link { get; set; }
        [JsonProperty("redirect_url")] public string RedirectUrl { get; set; }
        public long? Size { get; set; }
        [JsonProperty("status")] public string RawStatus { get; set; }
        [JsonIgnore] public UploadStatus Status => StringEnumConverterRepository.Default.GetValue<UploadStatus>(RawStatus);
        public string Uri { get; set; }
        [JsonProperty("upload_link")] public string UploadLink { get; set; }
        public User User { get; set; }
        [JsonProperty("ticket_id")] public string TicketId { get; set; }
        [JsonProperty("upload_link_secure")] public string UploadLinkSecure { get; set; }
    }
}
