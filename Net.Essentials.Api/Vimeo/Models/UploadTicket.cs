using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Text;

namespace Net.Essentials.Vimeo.Models
{
    public class UploadTicket
    {
        public string Uri { get; set; }
        public User User { get; set; }
        [JsonProperty("ticket_id")] public string TicketId { get; set; }
        [JsonProperty("upload_link")] public string UploadLink { get; set; }
        [JsonProperty("upload_link_secure")] public string UploadLinkSecure { get; set; }
        public string Form { get; set; }
    }
}
