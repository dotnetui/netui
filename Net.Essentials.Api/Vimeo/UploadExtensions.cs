using Net.Essentials.Components;
using Net.Essentials.Vimeo.Models;

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Net.Essentials.Vimeo
{
    public static class UploadExtensions
    {
        public static async Task<UploadTicket> CreateVideoAsync(
            this VimeoClient client, long size, UploadApproach approach = UploadApproach.Tus, string url = default, Dictionary<string, object> metadata = default)
        {
            var upload = new Dictionary<string, object>
            {
                { "approach", approach.ToString().ToLower() },
                { "size", size.ToString() }
            };
            if (url != null)
                upload.Add(approach == UploadApproach.Post ? "redirect_url" : "link", WebUtility.UrlEncode(url));

            Dictionary<string, object> payload = metadata == null ? new Dictionary<string, object>() : new Dictionary<string, object>(metadata);
            payload["upload"] = upload;

            var result = await client.RequestAsync<UploadTicket>("/me/videos", HttpMethod.Post, payload);
            return result;
        }

        public static async Task<bool> UploadTusChunkAsync(this VimeoClient client, string uploadLink, string path, long offset, long take = -1)
        {
            var tcpUploader = new TcpUploader();
            tcpUploader.OnProgress += (object s, EventArgs e) => Console.WriteLine($"Progress: {tcpUploader.Position}/{tcpUploader.Total} {tcpUploader.IsActive}");
            tcpUploader.Headers.Add("Tus-Resumable", "1.0.0");
            tcpUploader.Headers.Add("Upload-Offset", offset.ToString());
            tcpUploader.ContentType = "application/offset+octet-stream";
            tcpUploader.Method = "PATCH";
            tcpUploader.Url = uploadLink;
            tcpUploader.StreamBytes = await LoadFileChunkAsync(path, offset, take);
            var result = await tcpUploader.ExecuteAsync();
            return result != null;
        }

        static async Task<byte[]> LoadFileChunkAsync(string path, long offset, long take = -1)
        {
            byte[] buffer = null;
            await Task.Run(() =>
            {
                using (var fi = File.OpenRead(path))
                {
                    fi.Seek(offset, SeekOrigin.Begin);
                    if (take < 0 || fi.Length - offset < take) take = fi.Length - offset;
                    if (take > 0)
                    {
                        buffer = new byte[take];
                        fi.Read(buffer, 0, buffer.Length);
                    }
                }
            });
            return buffer;
        }
    }
}
