namespace Net;

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
}