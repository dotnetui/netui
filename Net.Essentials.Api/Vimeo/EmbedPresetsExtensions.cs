using Net.Essentials.Vimeo.Models;

using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Net.Essentials.Vimeo
{
    public static class EmbedPresetsExtensions
    {
        public static async Task<Preset> CreatePresetAsync(this VimeoClient client, string userId, object payload)
        {
            var response = await client.RequestAsync<Preset>($"/users/{userId}/presets", HttpMethod.Post, payload);
            return response;
        }
        public static async Task<Preset> CreatePresetAsync(this VimeoClient client, object payload)
        {
            var response = await client.RequestAsync<Preset>($"/me/presets", HttpMethod.Post, payload);
            return response;
        }

        public static async Task<Preset> EditPresetAsync(this VimeoClient client, string userId, string presetId, object payload)
        {
            var response = await client.RequestAsync<Preset>($"/users/{userId}/presets/{presetId}", HttpMethod.Patch, payload);
            return response;
        }

        public static async Task<Preset> EditPresetAsync(this VimeoClient client, string presetId, object payload)
        {
            var response = await client.RequestAsync<Preset>($"/me/presets/{presetId}", HttpMethod.Patch, payload);
            return response;
        }

        public static async Task<Preset> GetPresetAsync(this VimeoClient client, string userId, string presetId)
        {
            var response = await client.RequestAsync<Preset>($"/users/{userId}/presets/{presetId}");
            return response;
        }

        public static async Task<Preset> GetPresetAsync(this VimeoClient client, string presetId)
        {
            var response = await client.RequestAsync<Preset>($"/me/presets/{presetId}");
            return response;
        }

        public static async Task<Collection<Preset>> GetPresetsForUserAsync(this VimeoClient client, string userId, int? page = default, int? perPage = default)
        {
            return await client.PaginatedRequestAsync<Preset>($"/users/{userId}/presets", page: page, perPage: perPage);
        }

        public static async Task<Collection<Preset>> GetPresetsAsync(this VimeoClient client, int? page = default, int? perPage = default)
        {
            return await client.PaginatedRequestAsync<Preset>($"/me/presets", page: page, perPage: perPage);
        }

        public static async Task<Pictures> AddCustomUserLogoAsync(this VimeoClient client, string userId)
        {
            var response = await client.RequestAsync<Pictures>($"/users/{userId}/customlogos", HttpMethod.Post);
            return response;
        }

        public static async Task<Pictures> AddCustomUserLogoAsync(this VimeoClient client)
        {
            var response = await client.RequestAsync<Pictures>($"/me/customlogos", HttpMethod.Post);
            return response;
        }

        public static async Task<bool?> DeleteCustomUserLogoAsync(this VimeoClient client, string userId, string logoId)
        {
            var response = await client.RequestAsync($"/users/{userId}/customlogos/{logoId}", HttpMethod.Delete);
            if (response == null || response.StatusCode == 0) return default;
            return response.StatusCode == HttpStatusCode.NoContent;
        }
        
        public static async Task<bool?> DeleteCustomUserLogoAsync(this VimeoClient client, string logoId)
        {
            var response = await client.RequestAsync($"/me/customlogos/{logoId}", HttpMethod.Delete);
            if (response == null || response.StatusCode == 0) return default;
            return response.StatusCode == HttpStatusCode.NoContent;
        }

        public static async Task<Pictures> GetCustomUserLogoAsync(this VimeoClient vimeoClient, string userId, string logoId)
        {
            var response = await vimeoClient.RequestAsync<Pictures>($"/users/{userId}/customlogos/{logoId}");
            return response;
        }

        public static async Task<Pictures> GetCustomUserLogoAsync(this VimeoClient vimeoClient, string logoId)
        {
            var response = await vimeoClient.RequestAsync<Pictures>($"/me/customlogos/{logoId}");
            return response;
        }

        public static async Task<Pictures[]> GetCustomUserLogosAsync(this VimeoClient client, string userId)
        {
            var response = await client.RequestAsync<Pictures[]>($"/users/{userId}/customlogos");
            return response;
        }

        public static async Task<Pictures[]> GetCustomUserLogosAsync(this VimeoClient client)
        {
            var response = await client.RequestAsync<Pictures[]>($"/me/customlogos");
            return response;
        }

        public static async Task<Pictures> AddTimelineThumbnailAsync(this VimeoClient client, string videoId)
        {
            var response = await client.RequestAsync<Pictures>($"/videos/{videoId}/timelinethumbnails", HttpMethod.Post);
            return response;
        }

        public static async Task<Pictures> GetTimelineThumbnailAsync(this VimeoClient client, string videoId, string thumbnailId)
        {
            var response = await client.RequestAsync<Pictures>($"/videos/{videoId}/timelinethumbnails/{thumbnailId}");
            return response;
        }

        public static async Task<bool?> AddPresetToVideoAsync(this VimeoClient client, string videoId, string presetId)
        {
            var response = await client.RequestAsync($"/videos/{videoId}/presets/{presetId}", HttpMethod.Put);
            if (response == null || response.StatusCode == 0) return default;
            return response.StatusCode == HttpStatusCode.NoContent;
        }

        public static async Task<bool?> CheckVideoHasPresetAsync(this VimeoClient client, string videoId, string presetId)
        {
            var response = await client.RequestAsync($"/videos/{videoId}/presets/{presetId}");
            if (response == null || response.StatusCode == 0) return default;
            return response.StatusCode == HttpStatusCode.NoContent;
        }

        public static async Task<bool?> RemovePresetFromVideoAsync(this VimeoClient client, string videoId, string presetId)
        {
            var response = await client.RequestAsync($"/videos/{videoId}/presets/{presetId}", HttpMethod.Delete);
            if (response == null || response.StatusCode == 0) return default;
            return response.StatusCode == HttpStatusCode.NoContent;
        }

        public static async Task<Collection<Video>> GetUserVideosWithPresetAsync(this VimeoClient client, string userId, string presetId, int? page = default, int? perPage = default)
        {
            return await client.PaginatedRequestAsync<Video>($"users/{userId}/presets/{presetId}/videos", page: page, perPage: perPage);
        }

        public static async Task<Collection<Video>> GetVideosWithPresetAsync(this VimeoClient client, string presetId, int? page = default, int? perPage = default)
        {
            return await client.PaginatedRequestAsync<Video>($"me/presets/{presetId}/videos", page: page, perPage: perPage);
        }
    }
}
