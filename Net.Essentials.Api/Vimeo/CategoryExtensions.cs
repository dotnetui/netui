using Net.Essentials.Vimeo.Models;

using RestSharp;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Net.Essentials.Vimeo
{
    public static class CategoryExtensions
    {
        public static async Task<Category> GetCategoryAsync(this VimeoClient client, string category)
        {
            return await client.RequestAsync<Category>($"/categories/{category}");
        }

        public static async Task<Collection<Category>> GetCategoriesAsync(this VimeoClient client,
            SortDirection direction = default,
            int? page = default,
            int? perPage = default,
            string sort = default)
        {
            return await client.PaginatedRequestAsync<Category>($"/categories",
                direction: direction,
                page: page,
                perPage: perPage,
                sort: sort);
        }

        public static async Task<Collection<Channel>> GetCategoryChannelsAsync(this VimeoClient client,
            string category,
            SortDirection direction = default,
            int? page = default,
            int? perPage = default,
            string sort = default,
            string query = default)
        {
            return await client.PaginatedRequestAsync<Channel>($"/categories/{category}/channels",
                direction: direction,
                page: page,
                perPage: perPage,
                sort: sort,
                query: query);
        }

        public static async Task<Collection<Group>> GetCategoryGroupsAsync(this VimeoClient client,
            string category,
            SortDirection direction = default,
            int? page = default,
            int? perPage = default,
            string sort = default,
            string query = default)
        {
            return await client.PaginatedRequestAsync<Group>($"/categories/{category}/groups",
                direction: direction,
                page: page,
                perPage: perPage,
                sort: sort,
                query: query);
        }

        public static async Task<bool> FollowCategoryAsync(this VimeoClient client, string userId, string category)
        {
            return (await client.RequestAsync($"/users/{userId}/categories/{category}", HttpMethod.Put))?.StatusCode == HttpStatusCode.NoContent;
        }
        
        public static async Task<bool> FollowCategoryAsync(this VimeoClient client, string category)
        {
            return (await client.RequestAsync($"/me/categories/{category}", HttpMethod.Put))?.StatusCode == HttpStatusCode.NoContent;
        }

        public static async Task<bool> UnfollowCategoryAsync(this VimeoClient client, string userId, string category)
        {
            return (await client.RequestAsync($"/users/{userId}/categories/{category}", HttpMethod.Delete))?.StatusCode == HttpStatusCode.NoContent;
        }

        public static async Task<bool> UnfollowCategoryAsync(this VimeoClient client, string category)
        {
            return (await client.RequestAsync($"/me/categories/{category}", HttpMethod.Delete))?.StatusCode == HttpStatusCode.NoContent;
        }

        public static async Task<bool> CheckIfUserFollowsCategoryAsync(this VimeoClient client, string userId, string category)
        {
            return (await client.RequestAsync($"/users/{userId}/categories/{category}"))?.StatusCode == HttpStatusCode.NoContent;
        }

        public static async Task<bool> CheckIfUserFollowsCategoryAsync(this VimeoClient client, string category)
        {
            return (await client.RequestAsync($"/me/categories/{category}"))?.StatusCode == HttpStatusCode.NoContent;
        }

        public static async Task<Collection<Category>> GetCategoriesFollowedByUserAsync(this VimeoClient client, string userId,
            SortDirection direction = default,
            int? page = default,
            int? perPage = default,
            string sort = default)
        {
            return await client.PaginatedRequestAsync<Category>($"/users/{userId}/categories",
                direction: direction,
                page: page,
                perPage: perPage,
                sort: sort);
        }

        public static async Task<Collection<Category>> GetCategoriesFollowedByUserAsync(this VimeoClient client,
            SortDirection direction = default,
            int? page = default,
            int? perPage = default,
            string sort = default)
        {
            return await client.PaginatedRequestAsync<Category>($"/me/categories",
                direction: direction,
                page: page,
                perPage: perPage,
                sort: sort);
        }

        public static async Task<Video[]> GetVideoInCategoryAsync(this VimeoClient client, string category, string videoId)
        {
            var response = await client.RequestAsync($"/categories/{category}/videos/{videoId}");
            if (response?.StatusCode == HttpStatusCode.NotFound)
                return Array.Empty<Video>();
            return new[] { client.DeserializeResponse<Video>(response) };
        }

        public static async Task<Collection<Category>> GetCategoriesContainingVideoAsync(this VimeoClient client, string videoId,
            int? page = default,
            int? perPage = default)
        {
            return await client.PaginatedRequestAsync<Category>($"/videos/{videoId}/categories",
                page: page,
                perPage: perPage);
        }

        public static async Task<Collection<Video>> GetCategoryVideosAsync(this VimeoClient client, string category,
            SortDirection direction = default,
            int? page = default,
            int? perPage = default,
            string sort = default,
            string filter = default,
            bool? filterEmbeddable = default,
            string query = default)
        {
            return await client.PaginatedRequestAsync<Video>($"/categories/{category}/videos",
                direction: direction,
                page: page,
                perPage: perPage,
                sort: sort,
                filter: filter,
                requestBuilder: req =>
                {
                    if (filterEmbeddable.HasValue)
                        req.AddQueryParameter("filter_embeddable", filterEmbeddable.Value.ToString().ToLower());
                    if (!string.IsNullOrWhiteSpace(query))
                        req.AddQueryParameter("query", query);
                });
        }

        public static async Task<SetCategoriesForVideoResponse> SetCategoriesForVideoAsync(this VimeoClient client, string videoId, string[] categories)
        {
            var payload = categories.Select(x => new { Category = x }).ToArray();
            var response = await client.RequestAsync($"/videos/{videoId}/categories", HttpMethod.Put, payload);
            if (response == default || response.StatusCode == 0) return SetCategoriesForVideoResponse.None;
            if (response.StatusCode == HttpStatusCode.Created) return SetCategoriesForVideoResponse.Created;
            if (response.StatusCode == HttpStatusCode.NotFound) return SetCategoriesForVideoResponse.NotFound;
            if (response.StatusCode == HttpStatusCode.Forbidden) return SetCategoriesForVideoResponse.UserDoesntOwnVideo;
            return SetCategoriesForVideoResponse.Error;
        }
    }
}
