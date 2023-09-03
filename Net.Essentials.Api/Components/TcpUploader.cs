using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;

namespace Net.Essentials.Components
{
    public class TcpUploaderResponse
    {
        public string Text { get; set; }
        public string[] Fields { get; set; }

        public static implicit operator string(TcpUploaderResponse r)
        {
            return r?.Text;
        }
    }

    public class TcpUploader
    {
        public WebHeaderCollection Headers = new WebHeaderCollection();
        public byte[] StreamBytes;
        public long ContentLength => StreamBytes?.Length ?? 0;
        public string ContentType = "application/octet-stream";
        public string Method = "POST";
        public string Accept;
        public string Host;
        public string Url;
        public SslProtocols SslProtocols = SslProtocols.Default;
        public int ChunkSize = 1024 * 1024;
        
        public int Position { get; private set; }
        public int Total { get; private set; }

        public event EventHandler OnProgress;
        public event EventHandler OnFinish;

        volatile bool _isActive = false;
        public bool IsActive
        {
            private set => _isActive = value;
            get => _isActive;
        }

        public void Cancel()
        {
            IsActive = false;
        }

        public async Task<TcpUploaderResponse> ExecuteAsync()
        {
            var response = new TcpUploaderResponse();
            IsActive = true;
            try
            {
                await Task.Run(async () =>
                {
                    var uri = new Uri(Url);
                    if (string.IsNullOrEmpty(Host)) Host = uri.Host;

                    var sb = new StringBuilder();
                    sb.Append($"{Method} {Url} HTTP/1.1\r\n");
                    sb.Append($"Host: {Host}\r\n");
                    sb.Append("Connection: Close\r\n");

                    if (!string.IsNullOrWhiteSpace(Accept))
                        sb.Append($"Accept: {Accept}\r\n");
                    if (!string.IsNullOrWhiteSpace(ContentType))
                        sb.Append($"Content-Type: {ContentType}\r\n");

                    if (Headers != null)
                    {
                        for (int i = 0; i < Headers.Count; ++i)
                        {
                            var key = Headers.GetKey(i);
                            foreach (var value in Headers.GetValues(i))
                                sb.Append($"{key}: {value}\r\n");
                        }
                    }

                    sb.Append($"Content-Length: {ContentLength}\r\n");
                    sb.Append("\r\n");

                    var header = Encoding.UTF8.GetBytes(sb.ToString());

                    string responseFromServer = "";
                    int responseLength = 0;
                    response.Fields = new string[0];

                    async Task UploadAsync(Stream stream)
                    {
                        await stream.WriteAsync(header, 0, header.Length);
                        await stream.FlushAsync();
                        Position = 0;
                        while (Position < StreamBytes.Length)
                        {
                            if (!IsActive) break;
                            var sz = Math.Min(StreamBytes.Length - Position, ChunkSize);
                            if (sz > 0)
                            {
                                await stream.WriteAsync(StreamBytes, Position, sz);
                                await stream.FlushAsync();
                            }
                            Position += sz;
                            OnProgress?.Invoke(this, EventArgs.Empty);
                        }
                        while (true)
                        {
                            var responseBytes = new byte[8192];
                            var bytesCount = await stream.ReadAsync(responseBytes, 0, responseBytes.Length);
                            if (bytesCount == 0) break;
                            responseFromServer += Encoding.UTF8.GetString(responseBytes, 0, bytesCount);
                            responseLength += bytesCount;
                        }
                    }

                    using (var client = new TcpClient())
                    {
                        if (uri.Scheme.ToLower() == "https")
                        {
                            await client.ConnectAsync(Host, 443);
                            using (var stream = new SslStream(
                                client.GetStream(),
                                false,
                                new RemoteCertificateValidationCallback((sender, certificate, chain, policyErrors) => true)))
                            {
                                stream.AuthenticateAsClient(Host, null, SslProtocols, false);
                                await UploadAsync(stream);
                            }
                        }
                        else //:http
                        {
                            await client.ConnectAsync(Host, 80);
                            using (var stream = client.GetStream())
                            {
                                await UploadAsync(stream);
                            }
                        }
                        client.Close();
                    }

                    var bodyPos = responseFromServer.IndexOf("\r\n\r\n");
                    if (bodyPos >= 0)
                    {
                        response.Fields = responseFromServer.Substring(0, bodyPos).Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                        bodyPos += 4;
                        responseFromServer = responseFromServer.Substring(bodyPos, responseFromServer.Length - bodyPos);
                    }

                    response.Text = responseFromServer;
                    OnFinish?.Invoke(this, EventArgs.Empty);
                });
                return response;
            }
            catch
            {
                throw;
            }
            finally
            {
                IsActive = false;
            }
        }
    }
}
