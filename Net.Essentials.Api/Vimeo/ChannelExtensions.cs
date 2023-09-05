using Net.Essentials.Vimeo.Models;

using RestSharp;

using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Net.Essentials.Vimeo
{
    public enum DeleteChannelResponse
    {
        None,
        Deleted,
        Forbidden,
        Error
    }

    public enum AddChannelToCategoryResponse
    {
        None,
        Added,
        BadRequest,
        Unauthorized,
        Forbidden,
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

        public static async Task<DeleteChannelResponse> DeleteChannelAsync(this VimeoClient client, string channelId)
        {
            var response = await client.RequestAsync($"/channels/{channelId}", HttpMethod.Delete);
            if (response == null || response.StatusCode == 0)
                return DeleteChannelResponse.None;
            switch (response.StatusCode)
            {
                case System.Net.HttpStatusCode.NoContent:
                    return DeleteChannelResponse.Deleted;
                case System.Net.HttpStatusCode.Forbidden:
                    return DeleteChannelResponse.Forbidden;
                default:
                    return DeleteChannelResponse.Error;
            }
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

        public static async Task<AddChannelToCategoryResponse> AddChannelToCategoriesAsync(this VimeoClient client,
            string channelId,
            string[] categoryIds)
        {
            var response = await client.RequestAsync($"/channels/{channelId}/categories", HttpMethod.Put, new { category = categoryIds });
            return TranslateAddChannelToCategoryResponse(response);
        }

        public static async Task<AddChannelToCategoryResponse> AddChannelToCategoryAsync(this VimeoClient client,
            string channelId,
            string category)
        {
            var response = await client.RequestAsync($"/channels/{channelId}/categories/{category}", HttpMethod.Put);
            return TranslateAddChannelToCategoryResponse(response);
        }

        static AddChannelToCategoryResponse TranslateAddChannelToCategoryResponse(RestResponse response)
        {
            if (response == null || response.StatusCode == 0)
                return AddChannelToCategoryResponse.None;
            if (response.StatusCode == HttpStatusCode.NoContent)
                return AddChannelToCategoryResponse.Added;
            if (response.StatusCode == HttpStatusCode.BadRequest)
                return AddChannelToCategoryResponse.BadRequest;
            if (response.StatusCode == HttpStatusCode.Unauthorized)
                return AddChannelToCategoryResponse.Unauthorized;
            if (response.StatusCode == HttpStatusCode.Forbidden)
                return AddChannelToCategoryResponse.Forbidden;
            if (response.StatusCode == HttpStatusCode.NotFound)
                return AddChannelToCategoryResponse.NotFound;
            return AddChannelToCategoryResponse.Error;
        }
    }
}
