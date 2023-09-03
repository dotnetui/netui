using Net.Essentials.Converters;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Text;

namespace Net.Essentials.Vimeo.Models
{
    public class Video
    {
        [JsonProperty("allowed_privacies")] public string RawAllowedPrivacies { get; set; }
        public AppProfile App { get; set; }
        [JsonProperty("can_move_to_project")] public bool CanMoveToProject { get; set; }
        public Category[] Categories { get; set; }
        // connections.resource_creator_team_user
        // connections.resource_creator_team_user.uri
        // connections.team_permissions
        // connections.team_permissions.options
        [JsonProperty("content_rating")] public string[] RawContentRating { get; set; }
        [JsonIgnore] public ContentFilters ContentRating => ContentFiltersExtensions.ToContentFilters(RawContentRating);
        [JsonProperty("content_rating_class")] public string RawContentRatingClass { get; set; }
        [JsonIgnore] public ContentRatingClass ContentRatingClass => StringEnumConverterRepository.Default.GetValue<ContentRatingClass>(RawContentRatingClass);
        public object Context { get; set; }
        [JsonProperty("created_time")] public DateTimeOffset CreatedTime { get; set; }
        [JsonProperty("custom_url")] public string CustomUrl { get; set; }
        [JsonProperty("description")] public string Description { get; set; }
        [JsonProperty("disabled_properties")] public object DisabledProperties { get; set; }
        public VideoFile[] Download { get; set; }
        public int Duration { get; set; }
        [JsonProperty("edit_session")] public EditingSession EditSession { get; set; }
        public EmbedSettings Embed { get; set; }
        public VideoFile[] Files { get; set; }
        [JsonProperty("has_audio")] public bool HasAudio { get; set; }
        [JsonProperty("has_chapters")] public bool HasChapters { get; set; }
        [JsonProperty("has_text_tracks")] public bool HasTextTracks { get; set; }
        [JsonProperty("height")] public int Height { get; set; }
        [JsonProperty("is_copyright_restricted")] public bool IsCopyRightRestricted { get; set; }
        [JsonProperty("is_free")] public bool IsFree { get; set; }
        [JsonProperty("is_playable")] public bool IsPlayable { get; set; }
        [JsonProperty("language")] public string Language { get; set; }
        [JsonProperty("last_user_action_event_date")] public DateTimeOffset LastUserActionEventDate { get; set; }
        [JsonProperty("license")] public string License { get; set; }
        public string Link { get; set; }
        [JsonProperty("manage_link")] public string ManageLink { get; set; }
        public object Metadata { get; set; }
        public string Name { get; set; }
        [JsonProperty("parent_folder")] public Project ParentFolder { get; set; }
        public string Password { get; set; }
        public Pictures Pictures { get; set; }
        public Play Play { get; set; }
        [JsonProperty("player_embed_url")] public string PlayerEmbedUrl { get; set; }
        public Privacy Privacy { get; set; }
        [JsonProperty("rating_mod_locked")] public bool RatingModLocked { get; set; }
        [JsonProperty("release_time")] public DateTimeOffset ReleaseTime { get; set; }
        [JsonProperty("resource_key")] public string ResourceKey { get; set; }
        [JsonProperty("show_review_page")] public bool ShowReviewPage { get; set; }
        [JsonProperty("show_svv_footer_banner")] public bool ShowSvvFooterBanner { get; set; }
        [JsonProperty("show_svv_timecoded_comments")] public bool ShowSvvTimecodedComments { get; set; }
        public object Spatial { get; set; }
        public object Stats { get; set; }
        [JsonProperty("status")] public string RawStatus { get; set; }
        [JsonIgnore] public VideoAvailabilityStatus Status => StringEnumConverterRepository.Default.GetValue<VideoAvailabilityStatus>(RawStatus);
        public Tag[] Tags { get; set; }
        public Transcode Transcode { get; set; }
        public Transcript Transcript { get; set; }
        [JsonProperty("type")] public string RawType { get; set; }
        [JsonIgnore] public VideoType Type => StringEnumConverterRepository.Default.GetValue<VideoType>(RawType);
        [JsonProperty("upload")] public UploadTicket Upload { get; set; }
        public User Uploader { get; set; }
        public string Uri { get; set; }
        public User User { get; set; }
        [JsonProperty("version_transcode_status")] public object RawVersionTranscodeStatus { get; set; }
        public object Vod { get; set; }
        public int Width { get; set; }
    }
}
