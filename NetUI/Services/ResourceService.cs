using System.Reflection;

[assembly: Dependency(typeof(ResourceService))]
namespace Net.Essentials;
public class ResourceService
{
    readonly Dictionary<(Assembly, string), string> resourceNames = new Dictionary<(Assembly, string), string>();
    readonly Dictionary<string, Assembly> assemblies = new Dictionary<string, Assembly>();

    public Assembly GetAssembly()
    {
        return GetType().Assembly;
    }

    public Assembly GetAssembly(string query)
    {
        if (string.IsNullOrEmpty(query))
            return null;

        if (!assemblies.ContainsKey(query))
            assemblies[query] = AppDomain.CurrentDomain.GetAssemblies()
                .OrderBy(x => x.GetName().Name.Length)
                .FirstOrDefault(x => x.GetName().Name.Contains(query));

        return assemblies[query];
    }

    public string FindResourceName(string query, Assembly assembly)
    {
        if (string.IsNullOrWhiteSpace(query))
            return null;

        if (!resourceNames.ContainsKey((assembly, query)))
            resourceNames[(assembly, query)] = assembly
                .GetManifestResourceNames()
                .OrderBy(x => x.Length)
                .FirstOrDefault(x => x.Contains(query));

        return resourceNames[(assembly, query)];
    }

    public Stream GetResourceStream(string query, Assembly assembly)
    {
        var name = FindResourceName(query, assembly);
        return assembly.GetManifestResourceStream(name);
    }

    public string GetTextResourceStream(string query, Assembly assembly)
    {
        using (var stream = GetResourceStream(query, assembly))
        using (var reader = new StreamReader(stream))
            return reader.ReadToEnd();
    }

    public string GetRandomFileName(string prefix = "", string suffix = "")
    {
        var guid = Guid.NewGuid().ToString();
        var fileName = prefix + guid + suffix;
        return fileName;
    }

    public string GetSharedFilePath(string fileName = null)
    {
        var path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        if (!string.IsNullOrWhiteSpace(fileName))
            path = Path.Combine(path, fileName);
        return path;
    }

    public string GetRandomFilePath(string prefix = "", string suffix = "")
    {
        return GetSharedFilePath(GetRandomFileName(prefix, suffix));
    }

    public async Task SaveStreamAsync(Stream stream, string path)
    {
        using var fileStream = File.Create(path);
        stream.Seek(0, SeekOrigin.Begin);
        await stream.CopyToAsync(fileStream);
    }

    public void SaveStream(Stream stream, string path)
    {
        using var fileStream = File.Create(path);
        stream.Seek(0, SeekOrigin.Begin);
        stream.CopyTo(fileStream);
    }

    public async Task ShareAsync(string path, string title = null)
    {
        await Share.RequestAsync(new ShareFileRequest
        {
            Title = title ?? Path.GetFileName(path),
            File = new ShareFile(path)
        });
    }
}
