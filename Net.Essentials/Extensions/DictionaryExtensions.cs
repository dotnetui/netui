namespace Net;

public static class DictionaryExtensions
{
    public static Dictionary<string, string> Merge(this Dictionary<string, string> source, Dictionary<string, string> dest)
    {
        return Merge(source, null, dest);
    }

    public static Dictionary<string, string> Merge(this Dictionary<string, string> source, string? prefix, Dictionary<string, string> dest)
    {
        if (dest == null)
            throw new ArgumentNullException(nameof(dest));
        if (source == null)
            return dest;
        foreach (var item in source)
            dest[$"{prefix}{item.Key}"] = item.Value;
        return dest;
    }

    public static Dictionary<string, string>? Flatten(this Dictionary<string, object> source)
    {
        if (source == null)
            return null;
        var variables = new Dictionary<string, string>();
        foreach (var item in source)
            variables[item.Key] = item.Value?.ToString() ?? "";
        return variables;
    }

    public static Dictionary<string, string>? Bracketized(this Dictionary<string, string>? variables, string open = "{{", string close = "}}")
    {
        return variables?.ToDictionary(x => open + x.Key + close, y => y.Value) ?? variables;
    }
}