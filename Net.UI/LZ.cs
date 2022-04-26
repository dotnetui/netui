namespace Net.Essentials;

public static class L
{
    public static string CurrentLanguage { get; private set; } = "En";

    public static string LDayOfWeek(this DateTime dt)
    {
        return T(dt.DayOfWeek.ToString());
    }

    static Dictionary<string, Dictionary<string, string?>> keys = new();

    public static Dictionary<string, Dictionary<string, string?>> DynamicKeys = new();

    public static Dictionary<string, Dictionary<string, string?>> GetEmbeddedKeys() => keys;

    public static Dictionary<string, Dictionary<string, string?>> GetEffectiveKeys()
    {
        var result = new Dictionary<string, Dictionary<string, string?>>();
        if (DynamicKeys != null)
        {
            foreach (var item in DynamicKeys)
            {
                result[item.Key] = new();
                foreach (var pair in item.Value)
                    result[item.Key][pair.Key] = pair.Value;
            }
        }

        foreach (var item in keys)
        {
            if (!result.ContainsKey(item.Key))
                result[item.Key] = new();
            foreach (var pair in item.Value)
            {
                if (!result[item.Key].ContainsKey(item.Key))
                    result[item.Key][pair.Key] = pair.Value;
            }
        }

        return result;
    }

    public static event EventHandler<string>? OnLanguageChange;

    static IEnumerable<string>? overrideKeys = null;

    public static void SetLanguageKeys(IEnumerable<string> keys)
    {
        overrideKeys = keys;
    }

    public static IEnumerable<string> GetLanguageKeys()
    {
        if (overrideKeys != null) return overrideKeys;

        var langs = keys.Keys;
        if (DynamicKeys == null) return langs;
        return new HashSet<string>(langs.Union(DynamicKeys.Keys));
    }

    public static void SetLanguage(string language)
    {
        CurrentLanguage = language;
        OnLanguageChange?.Invoke(language, language);
    }

    public static void SetLanguage(Enum language) => SetLanguage(language.ToString());

    public static void Load(Type rootType, string prefix, string extension = ".l.json")
    {
        var assembly = rootType.GetTypeInfo().Assembly;
        var resourceNames = assembly.GetManifestResourceNames();
        try
        {
            foreach (var resource in resourceNames.Where(x => x.ToLowerInvariant().EndsWith(extension)))
            {
                var lang = resource[..^extension.Length];
                lang = lang[prefix.Length..];
                keys[lang] = new();

                var suffix = $"{lang}{extension}";
                var r = resourceNames.FirstOrDefault(x => x.EndsWith(suffix));
                if (r == null) continue;

                var stream = assembly.GetManifestResourceStream(r);
                string text = "{}";
                if (stream != null)
                {
                    using var reader = new StreamReader(stream);
                    text = reader.ReadToEnd();
                }

                var dic = JsonConvert.DeserializeObject<Dictionary<string, string?>>(text);
                if (dic != null) keys[lang] = dic;
            }
        }
        catch (Exception ex)
        {
            throw new AggregateException($"Error loading localization info from the type. Available resources are: {string.Join(", ", resourceNames)}", ex);
        }
    }

    public static void Load(string languageKey, Dictionary<string, string?> map)
    {
        keys[languageKey] = map;
    }

    public static void Load(Dictionary<string, Dictionary<string, string?>> map)
    {
        keys = map;
    }

    /// <summary>
    /// Returns Translation for the current language, or the fallback language, or the key itself.
    /// </summary>
    /// <param name="key">The key to return translation for</param>
    /// <returns></returns>
    public static string T(string key, params string[] args)
    {
        if (string.IsNullOrWhiteSpace(key)) return key;
        var value = GetValue(CurrentLanguage, key, args);
        if (value != null) return value;
        foreach (string lang in GetLanguageKeys())
        {
            value = GetValue(lang, key, args);
            if (!string.IsNullOrWhiteSpace(value)) return value;
        }
        return key;
    }

    public static string Tn(string key0, string key1, string keyx, int count)
    {
        if (count == 0) return T(key0, count.ToString());
        if (count == 1) return T(key1, count.ToString());
        return T(keyx, count.ToString());
    }

    static string? GetValue(string lang, string key, params string[] args)
    {
        try
        {
            if (DynamicKeys != null)
            {
                //Try get remote key
                if (DynamicKeys.ContainsKey(lang) && (DynamicKeys[lang]?.ContainsKey(key) ?? false))
                {
                    var val = DynamicKeys[lang][key];
                    if (!string.IsNullOrWhiteSpace(val))
                    {
                        if (args == null || args.Length == 0) return val;
                        return string.Format(val, args);
                    }
                }
            }

            if (keys.ContainsKey(lang) && keys[lang].TryGetValue(key, out var value))
            {
                if (args == null || args.Length == 0 || value == null) return value;
                return string.Format(value, args);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in GetValue: {lang}, {key}, {ex}");
        }
        return null;
    }
}


[ContentProperty("Key")]
public class LExtension : IMarkupExtension
{
    public string Key { get; set; }
    public object ProvideValue(IServiceProvider serviceProvider)
    {
        return L.T(Key);
    }
}

[ContentProperty("Key")]
public class LuExtension : IMarkupExtension
{
    public string Key { get; set; }
    public object ProvideValue(IServiceProvider serviceProvider)
    {
        return L.T(Key)?.ToUpper();
    }
}