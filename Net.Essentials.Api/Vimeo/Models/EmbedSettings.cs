using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Text;

namespace Net.Essentials.Vimeo.Models
{
    public class EmbedSettings
    {
        public bool Airplay { get; set; }
        [JsonProperty("audio_tracks")] public bool AudioTracks { get; set; }
        public bool Autopip { get; set; }
        public object Badges { get; set; }
        public Buttons Buttons { get; set; }
        public object Cards { get; set; }
        public bool Chapters { get; set; }
        public bool Chromecast { get; set; }
        [JsonProperty("closed_captions")] public bool ClosedCaptions { get; set; }
        public object Colors { get; set; }
        [JsonProperty("email_capture_form")] public object EmailCaptureForm { get; set; }
        [JsonProperty("end_screen")] public object EndScreen { get; set; }
        [JsonProperty("event_schedule")] public bool EventSchedule { get; set; }
        [JsonProperty("has_cards")] public bool HasCards { get; set; }
        public string Html { get; set; }
        public bool Interactive { get; set; }
        public object Logos { get; set; }
        [JsonProperty("outro_type")] public string OutroType { get; set; }
        public bool Pip { get; set; }
        public bool Playbar { get; set; }
        [JsonProperty("show_timezone")] public bool ShowTimezone { get; set; }
        public bool Speed { get; set; }
        public object Title { get; set; }
        public bool Transcript { get; set; }
        public string Uri { get; set; }
        public bool Volume { get; set; }
    }
}
