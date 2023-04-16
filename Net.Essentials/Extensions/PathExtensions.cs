using System;
using System.IO;
using System.Security.Cryptography;

namespace Net.Essentials
{
    public static class PathExtensions
    {
        public static string CreateDirectoryAndReturn(this string path)
        {
            if (!string.IsNullOrWhiteSpace(path))
            {
                var directoryName = Path.GetDirectoryName(path);
                if (!string.IsNullOrWhiteSpace(directoryName))
                    Directory.CreateDirectory(directoryName);
            }
            return path;
        }

        public static string MakeFileNameLegal(string fileName)
        {
            fileName = fileName
                .Trim()
                .Replace("#%&{}<>*?$!'\":@+`|=", "_")
                .Replace("\\", "/");
            return fileName;
        }

        public static string CalculateMD5(string path)
        {
            using (var md5 = MD5.Create())
            using (var stream = File.OpenRead(path))
            {
                var hash = md5.ComputeHash(stream);
                return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
            }
        }

        public static string BytesCountToHumanReadable(long length)
        {
            var size = length / (double)(1024 * 1024);
            var result = $"{size:N1}M";
            if (result.EndsWith(".0M"))
                result = $"{(int)size}M";
            if (result == "0M")
                result = $"{size * 1024:N1}K";
            return result;
        }
    }
}