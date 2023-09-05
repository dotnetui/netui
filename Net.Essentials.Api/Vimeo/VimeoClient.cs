using Net.Essentials.Vimeo.JsonConverters;
using Net.Essentials.Vimeo.Models;

using Newtonsoft.Json;

using RestSharp;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Net.Essentials.Vimeo
{
    public partial class VimeoClient : ApiClient
    {
        public static bool IsOk(RestResponse response) => response != null &&
            response.StatusCode == HttpStatusCode.OK ||
            response.StatusCode == HttpStatusCode.Created ||
            response.StatusCode == HttpStatusCode.PartialContent ||
            response.StatusCode == HttpStatusCode.Accepted;

        public VimeoApp App { get; set; }
        public ApiToken Token { get; set; }

        public bool IsAuthorized => Token != null && !Token.IsExpired;

        public VimeoClient(VimeoApp app) : this()
        {
            App = app;
        }

        public VimeoClient(string accessToken) : this()
        {
            Token = new ApiToken
            {
                AccessToken = accessToken
            };
        }

        public VimeoClient()
        {
            BaseUrl = "https://api.vimeo.com";
        }

        public override RestRequest CreateRequest(string path, Method method)
        {
            var request = base.CreateRequest(path, method);
            if (Token?.AccessToken != null)
                request.AddHeader("Authorization", $"Bearer {Token.AccessToken}");
            else if (!string.IsNullOrWhiteSpace(App?.ClientSecret) && !string.IsNullOrWhiteSpace(App?.ClientId))
                request.AddHeader("Authorization", $"basic {Base64Encode(App.ClientId, App.ClientSecret)}");
            request.AddHeader("Accept", "application/vnd.vimeo.*+json;version=3.4");
            return request;
        }

        string Base64Encode(string clientId, string clientSecret)
        {
            var bytes = Encoding.UTF8.GetBytes($"{clientId}:{clientSecret}");
            return Convert.ToBase64String(bytes);
        }

        static readonly JsonSerializerSettings DeserializeSettings = new JsonSerializerSettings
        {
            Error = (s, e) =>
            {
                Console.WriteLine(e.ErrorContext.Error);
                e.ErrorContext.Handled = true;
            },
            Converters = new List<JsonConverter>
            {
                new ConnectionMetadataJsonConverter(),
                new PicturesJsonConverter(),
                new PrivacyPreferencesJsonConverter()
            }
        };

        public async Task<Collection<T>> PaginatedRequestAsync<T>(
            string path, 
            HttpMethod method = HttpMethod.Get, 
            object dto = default,
            SortDirection direction = default,
            int? page = default,
            int? perPage = default,
            string sort = default,
            string query = default,
            Action<RestRequest> requestBuilder = default) where T :  class, new()
        {
            return await RequestAsync<Collection<T>>(path, method, dto, buildRequest: req =>
            {
                if (direction != default) req.AddQueryParameter("direction", direction.ToString().ToLowerInvariant());
                if (page != default) req.AddQueryParameter("page", page.ToString());
                if (perPage != default) req.AddQueryParameter("per_page", perPage.ToString());
                if (sort != default) req.AddQueryParameter("sort", sort);
                if (query != default) req.AddQueryParameter("query", query);
                requestBuilder?.Invoke(req);
            });
        }

        public override T DeserializeResponse<T>(RestResponse response, Action onfail = null)
        {
            if (response == null) return default;
            if (IsOk(response))
            {
                try
                {
                    return JsonConvert.DeserializeObject<T>(response.Content ?? "", DeserializeSettings);
                }
                catch (Exception ex)
                {
                    Log($"RequestAsync deserialization failed: {ex}");
                }
            }

            if (response != null)
                throw new VimeoException(response.Content);

            return default;
        }

        public async Task<RestResponse> GetOpenApiSpecsAsync()
        {
            return await RequestAsync("/");
        }

        public async Task<User> GetUserByIdAsync(string id)
        {
            return await RequestAsync<User>($"users/{id}");
        }

        public async Task<User> GetMeAsync()
        {
            var result = await RequestAsync<User>("me");
            return result;
        }

        public async Task<Video> GetVideoByIdAsync(string id)
        {
            return await RequestAsync<Video>($"videos/{id}");
        }

        public async Task<Project> GetFolderByIdAsync(string userId, string folderId)
        {
            return await RequestAsync<Project>($"users/{userId}/projects/{folderId}");
        }
    }
}
