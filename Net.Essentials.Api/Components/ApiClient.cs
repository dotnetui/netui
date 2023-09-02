using System;
using RestSharp;
using System.Net;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Collections;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Net.Essentials
{
    public static class ApiClientExtensions
    {
        public static bool IsOk(this RestResponse response) =>
            response != null && response.StatusCode == HttpStatusCode.OK;
    }

    public enum HttpMethod
    {
        Get, Post, Put, Delete, Head, Options, Patch, Merge, Copy, Search,
        GET = Get,
        POST = Post,
        DELETE = Delete,
        HEAD = Head,
        OPTIONS = Options,
        PATCH = Patch,
        MERGE = Merge,
        COPY = Copy,
        SEARCH = Search
    }

    public abstract class ApiClient
    {
        public event EventHandler<string> OnLog;
        public event EventHandler<RestResponse> OnServerError;
        public event EventHandler<RestResponse> OnResponse;
        public event EventHandler<RestResponse> OnSuccess;
        public event EventHandler<RestResponse> OnGone;
        public event EventHandler<Exception> OnException;
        public bool LogResponse = true;
        public bool ThrowExceptions = false;

        protected virtual void Log(string message, [CallerMemberName] string method = null)
        {
            OnLog?.Invoke(method, message);
        }

        public virtual string BaseUrl { get; set; }
        public virtual Func<Dictionary<string, string>> AdditionalHeaders { get; set; }

        public ApiClient() { }

        public async Task<RestResponse> RequestAsync(
            string path,
            HttpMethod method = HttpMethod.Get,
            object dto = null,
            Action<RestRequest> buildRequest = null,
            string contentType = null)
        {
            return await RequestAsync(path, (Method)method, dto, buildRequest, contentType);
        }

        public virtual async Task<RestResponse> RequestAsync(
            string path,
            Method method = Method.Get,
            object dto = null,
            Action<RestRequest> buildRequest = null,
            string contentType = null)
        {
            try
            {
                if (LogResponse) Log($"Request: [/{path}] {method} {path}");
                RestClient client = CreateClient(BaseUrl);
                var request = CreateRequest(path, method);
                buildRequest?.Invoke(request);
                if (dto != null)
                {
                    if (method == Method.Get)
                    {
                        var json = JsonConvert.SerializeObject(dto);
                        var dic = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
                        if (dic != null)
                        {
                            foreach (var pair in dic)
                            {
                                if (pair.Value != null)
                                {
                                    if (!(pair.Value is string) && pair.Value is IEnumerable e)
                                    {
                                        foreach (var item in e)
                                        {
                                            request.AddQueryParameter(pair.Key, item.ToString());
                                        }
                                    }
                                    else
                                    {
                                        request.AddQueryParameter(pair.Key, pair.Value.ToString());
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        request.AddJsonBody(dto);
                    }
                }
                contentType = contentType ?? "application/json";
                request.AddHeader("Content-Type", contentType);
                request.AddHeader("Accept", contentType);
                Stopwatch watch = new Stopwatch();
                watch.Start();
                try
                {
                    var result = await client.ExecuteAsync(request);
                    OnResponse?.Invoke(this, result);

                    if (LogResponse) Log($"[{watch.Elapsed.TotalSeconds}s/{BaseUrl}] {request.Method} {result?.ResponseUri?.ToString() ?? path} response ({result?.StatusCode}): {result?.Content?.Head(1024)}\n");
                    if (result != null && result.StatusCode == HttpStatusCode.Gone)
                    {
                        OnGone?.Invoke(this, result);
                        return null;
                    }
                    if (result != null && result.StatusCode != HttpStatusCode.OK)
                        OnServerError?.Invoke(this, result);
                    if (result != null)
                        OnSuccess?.Invoke(this, result);
                    return result;
                }
                catch (Exception ex)
                {
                    OnException?.Invoke(this, ex);
                    if (LogResponse) Log($"Exception while executing task: {ex}");
                    if (ThrowExceptions) throw;
                }
                finally
                {
                    watch.Stop();
                }
            }
            catch (Exception ex)
            {
                OnException?.Invoke(this, ex);
                if (LogResponse) Log($"Exception while executing task: {ex}");
                if (ThrowExceptions) throw;
            }
            return null;
        }

        public async Task<T> RequestAsync<T>(string path, HttpMethod method = HttpMethod.Get, object dto = null, Action<RestRequest> buildRequest = null, Action onfail = null, string contentType = null)
        {
            return await RequestAsync<T>(path, (Method)method, dto, buildRequest, onfail, contentType);
        }

        public virtual async Task<T> RequestAsync<T>(string path, Method method, object dto = null, Action<RestRequest> buildRequest = null, Action onfail = null, string contentType = null)
        {
            var response = await RequestAsync(path, method, dto, buildRequest, contentType);
            if (response == null) return default;
            return DeserializeResponse<T>(response, onfail);
        }

        public virtual T DeserializeResponse<T>(RestResponse response, Action onfail = null)
        {
            if (response == null) return default;
            if (response.IsOk())
            {
                try
                {
                    return JsonConvert.DeserializeObject<T>(response.Content ?? "");
                }
                catch (Exception ex)
                {
                    Log($"RequestAsync deserialization failed: {ex}");
                }
            }
            if (response != null && response.StatusCode != (HttpStatusCode)400)
                onfail?.Invoke();
            return default;
        }

        protected virtual RestClient CreateClient(string url) => new RestClient(url ?? "");
        protected virtual RestRequest CreateRequest(string path, Method method)
        {
            var request = new RestRequest(path, method);

            if (AdditionalHeaders != null)
                foreach (var item in AdditionalHeaders())
                    request.AddHeader(item.Key, item.Value);

            return request;
        }
    }
}