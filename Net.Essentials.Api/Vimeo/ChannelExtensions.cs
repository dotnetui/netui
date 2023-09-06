using Net.Essentials.Vimeo.Models;

using RestSharp;

using System;
using System.Collections.Generic;
using System.Net;
using System.Linq;
using System.Threading.Tasks;

namespace Net.Essentials.Vimeo
{
    public enum DeleteChannelResponse
    {
        None,
        Deleted,
        Forbidden,
        NotFound,
        Error
    }

    public enum AddChannelResponse
    {
        None,
        Added,
        BadRequest,
        Unauthorized,
        Forbidden,
        NotFound,
        Error
    }

    public enum CheckChannelResponse
    {
        None,
        Verified,
        NotFound,
        Error
    }

    public static class ChannelExtensions
    {
        public static async Task<Channel> CreateChannelsAsync(this VimeoClient client, string name, ChannelPrivacy privacy, string description = default, string link = default)
        {
            Dictionary<string, string> dto = new Dictionary<string, string>
            {
                { "name", name },
            };
            if (privacy != default)
                dto.Add("privacy", privacy.ToString().ToLowerInvariant());
            if (description != default)
                dto.Add("description", description);
            if (link != default)
                dto.Add("link", link);
            return await client.RequestAsync<Channel>($"/channels", HttpMethod.Post, dto);
        }

        static DeleteChannelResponse TranslateDeleteChannelResponse(RestResponse response)
        {
            if (response == null || response.StatusCode == 0)
                return DeleteChannelResponse.None;
            switch (response.StatusCode)
            {
                case HttpStatusCode.NoContent:
                    return DeleteChannelResponse.Deleted;
                case HttpStatusCode.Forbidden:
                    return DeleteChannelResponse.Forbidden;
                case HttpStatusCode.NotFound:
                    return DeleteChannelResponse.NotFound;
                default:
                    return DeleteChannelResponse.Error;
            }
        }

        public static async Task<DeleteChannelResponse> DeleteChannelAsync(this VimeoClient client, string channelId)
        {
            var response = await client.RequestAsync($"/channels/{channelId}", HttpMethod.Delete);
            return TranslateDeleteChannelResponse(response);
        }

        public static async Task<Channel> EditChannelAsync(this VimeoClient client, string channelId, string description, string link, string name, ChannelPrivacy privacy)
        {
            var payload = new Dictionary<string, string>();
            if (description != null) payload.Add("description", description);
            if (link != null) payload.Add("link", link);
            if (name != null) payload.Add("name", name);
            if (privacy != default) payload.Add("privacy", privacy.ToString().ToLowerInvariant());
            return await client.RequestAsync<Channel>($"/channels/{channelId}", HttpMethod.Patch, payload);
        }

        public static async Task<Channel> GetChannelAsync(this VimeoClient client, string channelId)
        {
            return await client.RequestAsync<Channel>($"/channels/{channelId}");
        }

        public static async Task<Collection<Channel>> GetChannelsAsync(this VimeoClient client,
            SortDirection direction = default,
            int? page = default,
            int? perPage = default,
            string sort = default,
            string query = default,
            string filter = default)
        {
            return await client.PaginatedRequestAsync<Channel>($"/channels",
                direction: direction,
                page: page,
                perPage: perPage,
                query: query,
                sort: sort,
                filter: filter);
        }

        public static async Task<Collection<Channel>> GetUserSubscribedChannelsAsync(this VimeoClient client,
            string userId,
            SortDirection direction = default,
            int? page = default,
            int? perPage = default,
            string sort = default,
            string query = default,
            string filter = default)
        {
            return await client.PaginatedRequestAsync<Channel>($"/users/{userId}/channels",
                direction: direction,
                page: page,
                perPage: perPage,
                query: query,
                sort: sort,
                filter: filter);
        }

        public static async Task<Collection<Channel>> GetUserSubscribedChannelsAsync(this VimeoClient client,
            SortDirection direction = default,
            int? page = default,
            int? perPage = default,
            string sort = default,
            string query = default,
            string filter = default)
        {
            return await client.PaginatedRequestAsync<Channel>($"/me/channels",
                direction: direction,
                page: page,
                perPage: perPage,
                query: query,
                sort: sort,
                filter: filter);
        }

        public static async Task<AddChannelResponse> AddChannelToCategoriesAsync(this VimeoClient client,
            string channelId,
            string[] categoryIds)
        {
            var response = await client.RequestAsync($"/channels/{channelId}/categories", HttpMethod.Put, new { category = categoryIds });
            return TranslateAddChannelResponse(response);
        }

        public static async Task<AddChannelResponse> AddChannelToCategoryAsync(this VimeoClient client,
            string channelId,
            string category)
        {
            var response = await client.RequestAsync($"/channels/{channelId}/categories/{category}", HttpMethod.Put);
            return TranslateAddChannelResponse(response);
        }

        static AddChannelResponse TranslateAddChannelResponse(RestResponse response)
        {
            if (response == null || response.StatusCode == 0)
                return AddChannelResponse.None;
            if (response.StatusCode == HttpStatusCode.NoContent)
                return AddChannelResponse.Added;
            if (response.StatusCode == HttpStatusCode.BadRequest)
                return AddChannelResponse.BadRequest;
            if (response.StatusCode == HttpStatusCode.Unauthorized)
                return AddChannelResponse.Unauthorized;
            if (response.StatusCode == HttpStatusCode.Forbidden)
                return AddChannelResponse.Forbidden;
            if (response.StatusCode == HttpStatusCode.NotFound)
                return AddChannelResponse.NotFound;
            return AddChannelResponse.Error;
        }

        public static async Task<List<Category>> GetChannelCategoriesAsync(this VimeoClient client, string channelId)
        {
            return await client.RequestAsync<List<Category>>($"/channels/{channelId}/categories");
        }

        public static async Task<DeleteChannelResponse> RemoveChannelFromCategoryAsync(this VimeoClient client, string channelId, string category)
        {
            var response = await client.RequestAsync($"/channels/{channelId}/categories/{category}", HttpMethod.Delete); 
            return TranslateDeleteChannelResponse(response);
        }

        public static async Task<AddChannelResponse> AddChannelModeratorsAsync(this VimeoClient client, string channelId, params string[] user_uris)
        {
            var payload = user_uris.Select(u => new { user_uri = u }).ToArray();
            var response = await client.RequestAsync($"/channels/{channelId}/moderators", HttpMethod.Put, payload);
            return TranslateAddChannelResponse(response);
        }

        public static async Task<AddChannelResponse> AddChannelModeratorAsync(this VimeoClient client, string channelId, string userId)
        {
            var response = await client.RequestAsync($"/channels/{channelId}/moderators/{userId}", HttpMethod.Put);
            return TranslateAddChannelResponse(response);
        }

        public static async Task<User> GetChannelModeratorAsync(this VimeoClient client, string channelId, string userId)
        {
            return await client.RequestAsync<User>($"/channels/{channelId}/moderators/{userId}");
        }

        public static async Task<Collection<User>> GetChannelModeratorsAsync(this VimeoClient client, string channelId,
            SortDirection direction = default,
            int? page = default,
            int? perPage = default,
            string sort = default,
            string query = default)
        {
            return await client.PaginatedRequestAsync<User>($"/channels/{channelId}/moderators",
                direction: direction,
                page: page,
                perPage: perPage,
                sort: sort,
                query: query);
        }

        public static async Task<DeleteChannelResponse> RemoveChannelModeratorsAsync(this VimeoClient client, string channelId, params string[] user_uris)
        {
            var payload = user_uris.Select(u => new { user_uri = u }).ToArray();
            var response = await client.RequestAsync($"/channels/{channelId}/moderators", HttpMethod.Delete, payload);
            return TranslateDeleteChannelResponse(response);
        }

        public static async Task<DeleteChannelResponse> RemoveChannelModeratorAsync(this VimeoClient client, string channelId, string userId)
        {
            var response = await client.RequestAsync($"/channels/{channelId}/moderators/{userId}", HttpMethod.Delete);
            return TranslateDeleteChannelResponse(response);
        }

        public static async Task<AddChannelResponse> ReplaceChannelModeratorsAsync(this VimeoClient client, string channelId, params string[] user_uris)
        {
            var payload = user_uris.Select(u => new { user_uri = u }).ToArray();
            var response = await client.RequestAsync($"/channels/{channelId}/moderators", HttpMethod.Patch, payload);
            return TranslateAddChannelResponse(response);
        }

        public static async Task<Collection<User>> GetPrivateChannelUsersAsync(this VimeoClient client, string channelId,
            SortDirection direction = default,
            int? page = default,
            int? perPage = default)
        {
            return await client.PaginatedRequestAsync<User>($"/channels/{channelId}/privacy/users",
                direction: direction,
                page: page,
                perPage: perPage);
        }

        public static async Task<AddChannelResponse> PermitChannelPrivateAccessToUsersAsync(this VimeoClient client, string channelId, params string[] user_uris)
        {
            var payload = user_uris.Select(u => new { user_uri = u }).ToArray();
            var response = await client.RequestAsync($"/channels/{channelId}/privacy/users", HttpMethod.Put, payload);
            return TranslateAddChannelResponse(response);
        }

        public static async Task<AddChannelResponse> PermitChannelPrivateAccessToUserAsync(this VimeoClient client, string channelId, string userId)
        {
            var response = await client.RequestAsync($"/channels/{channelId}/privacy/users/{userId}", HttpMethod.Put);
            return TranslateAddChannelResponse(response);
        }

        public static async Task<DeleteChannelResponse> RestrictChannelPrivateAccessForUserAsync(this VimeoClient client, string channelId, string userId)
        {
            var response = await client.RequestAsync($"/channels/{channelId}/privacy/users/{userId}", HttpMethod.Delete);
            return TranslateDeleteChannelResponse(response);
        }

        static CheckChannelResponse TranslateCheckChannelResponse(RestResponse response)
        {
            if (response == null || response.StatusCode == 0)
                return CheckChannelResponse.None;
            if (response.StatusCode == HttpStatusCode.NoContent)
                return CheckChannelResponse.Verified;
            if (response.StatusCode == HttpStatusCode.NotFound)
                return CheckChannelResponse.NotFound;
            return CheckChannelResponse.Error;
        }

        public static async Task<CheckChannelResponse> CheckUserFollowsChannelAsync(this VimeoClient client, string channelId, string userId)
        {
            var response = await client.RequestAsync($"/users/{userId}/channels/{channelId}", HttpMethod.Get);
            return TranslateCheckChannelResponse(response);
        }

        public static async Task<CheckChannelResponse> CheckUserFollowsChannelAsync(this VimeoClient client, string channelId)
        {
            var response = await client.RequestAsync($"/me/channels/{channelId}", HttpMethod.Get);
            return TranslateCheckChannelResponse(response);
        }

        public static async Task<Collection<User>> GetChannelFollowersAsync(this VimeoClient client, 
            string channelId,
            string filter = "moderators",
            SortDirection direction = default,
            int? page = default,
            int? perPage = default,
            string sort = default,
            string query = default)
        {
            return await client.PaginatedRequestAsync<User>($"/channels/{channelId}/users",
                direction: direction,
                page: page,
                perPage: perPage,
                sort: sort,
                query: query,
                filter: filter);
        }

        public static async Task<CheckChannelResponse> SubscribeToChannelAsync(this VimeoClient client, string channelId, string userId)
        {
            var response = await client.RequestAsync($"/users/{userId}/channels/{channelId}", HttpMethod.Put);
            return TranslateCheckChannelResponse(response);
        }

        public static async Task<CheckChannelResponse> SubscribeToChannelAsync(this VimeoClient client, string channelId)
        {
            var response = await client.RequestAsync($"/me/channels/{channelId}", HttpMethod.Put);
            return TranslateCheckChannelResponse(response);
        }

        public static async Task<CheckChannelResponse> UnsubscribeFromChannelAsync(this VimeoClient client, string channelId, string userId)
        {
            var response = await client.RequestAsync($"/users/{userId}/channels/{channelId}", HttpMethod.Delete);
            return TranslateCheckChannelResponse(response);
        }

        public static async Task<CheckChannelResponse> UnsubscribeFromChannelAsync(this VimeoClient client, string channelId)
        {
            var response = await client.RequestAsync($"/me/channels/{channelId}", HttpMethod.Delete);
            return TranslateCheckChannelResponse(response);
        }

        public static async Task<AddChannelResponse> AddChannelTagsAsync(this VimeoClient client, string channelId, string[] tags)
        {
            var payload = tags.Select(t => new { tag = t }).ToArray();
            var response = await client.RequestAsync($"/channels/{channelId}/tags", HttpMethod.Put, payload);
            return TranslateAddChannelResponse(response);
        }

        public static async Task<AddChannelResponse> AddChannelTagAsync(this VimeoClient client, string channelId, string word)
        {
            var response = await client.RequestAsync($"/channels/{channelId}/tags/{word}", HttpMethod.Put);
            return TranslateAddChannelResponse(response);
        }
        public static async Task<CheckChannelResponse> CheckChannelTagAsync(this VimeoClient client, string channelId, string word)
        {
            var response = await client.RequestAsync($"/channels/{channelId}/tags/{word}", HttpMethod.Get);
            return TranslateCheckChannelResponse(response);
        }

        public static async Task<Tag[]> GetChannelTagsAsync(this VimeoClient client, string channelId)
        {
            var response = await client.RequestAsync<Tag[]>($"/channels/{channelId}/tags", HttpMethod.Get);
            return response;
        }


        public static async Task<DeleteChannelResponse> RemoveChannelTagAsync(this VimeoClient client, string channelId, string word)
        {
            var response = await client.RequestAsync($"/channels/{channelId}/tags/{word}", HttpMethod.Delete);
            return TranslateDeleteChannelResponse(response);
        }

        public static async Task<AddChannelResponse> AddVideosToChannelAsync(this VimeoClient client, string channelId, params string[] video_uris)
        {
            var payload = video_uris.Select(v => new { video_uri = v }).ToArray();
            var response = await client.RequestAsync($"/channels/{channelId}/videos", HttpMethod.Put, payload);
            return TranslateAddChannelResponse(response);
        }

        public static async Task<AddChannelResponse> AddVideoToChannelAsync(this VimeoClient client, string channelId, string videoId)
        {
            var response = await client.RequestAsync($"/channels/{channelId}/videos/{videoId}", HttpMethod.Put);
            return TranslateAddChannelResponse(response);
        }

        public static async Task<Video> GetChannelVideoAsync(this VimeoClient client, string channelId, string videoId)
        {
            var response = await client.RequestAsync<Video>($"/channels/{channelId}/videos/{videoId}", HttpMethod.Get);
            return response;
        }

        public static async Task<Channel[]> GetAvailableChannelsAsync(this VimeoClient client, string videoId)
        {
            var response = await client.RequestAsync<Channel[]>($"/videos/{videoId}/available_channels", HttpMethod.Get);
            return response;
        }

        public static async Task<Collection<Video>> GetChannelVideosAsync(this VimeoClient client, string channelId,
            string containing_url = default,
            string filter = default,
            bool? filter_embeddable = default,
            SortDirection direction = default,
            int? page = default,
            int? perPage = default,
            string sort = default,
            string query = default)
        {
            return await client.PaginatedRequestAsync<Video>($"/channels/{channelId}/videos",
                direction: direction,
                page: page,
                perPage: perPage,
                sort: sort,
                query: query,
                filter: filter,
                requestBuilder: req =>
                {
                    if (filter_embeddable.HasValue)
                        req.AddParameter("filter_embeddable", filter_embeddable.Value.ToString().ToLower());
                    if (containing_url != null)
                        req.AddParameter("containing_url", containing_url);
                });
        }

        public static async Task<DeleteChannelResponse> RemoveVideosFromChannelAsync(this VimeoClient client, string channelId, params string[] video_uris)
        {
            var payload = video_uris.Select(v => new { video_uri = v }).ToArray();
            var response = await client.RequestAsync($"/channels/{channelId}/videos", HttpMethod.Delete, payload);
            return TranslateDeleteChannelResponse(response);
        }

        public static async Task<DeleteChannelResponse> RemoveVideoFromChannelAsync(this VimeoClient client, string channelId, string videoId)
        {
            var response = await client.RequestAsync($"/channels/{channelId}/videos/{videoId}", HttpMethod.Delete);
            return TranslateDeleteChannelResponse(response);
        }
    }
}
