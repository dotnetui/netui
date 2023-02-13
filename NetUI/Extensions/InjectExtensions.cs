using System.Reflection;

namespace Net.Essentials;

[AttributeUsage(AttributeTargets.All)]
public class IgnoreInjectAttribute : Attribute
{
}

[AttributeUsage(AttributeTargets.All)]
public class ProjectAttribute : Attribute
{
}

[AttributeUsage(AttributeTargets.All)]
public class IgnoreProjectAttribute : Attribute
{
}

public enum ProjectMode
{
    AllButIgnored,
    Explicit
}

public static class InjectExtensions
{
    static readonly Dictionary<Type, PropertyInfo[]> properties = new();

    static PropertyInfo[] GetProperties(Type type)
    {
        if (!properties.ContainsKey(type))
            return properties[type] = type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        return properties[type];
    }

    public static void Inject(this object me, object destination, Action<Exception>? exceptionHandler = null)
    {
        var destProps = GetProperties(destination.GetType());
        var myProps = GetProperties(me.GetType());
        foreach (var item in myProps)
        {
            if (item.GetCustomAttributes(typeof(IgnoreInjectAttribute), true).Any())
                continue;
            var matchingFields = from x in destProps where x.Name == item.Name select x;
            if (!matchingFields.Any()) continue;
            try
            {
                matchingFields.First().SetValue(destination, item.GetValue(me));
            }
            catch (Exception ex)
            {
                exceptionHandler?.Invoke(ex);
            }
        }
    }

    public static void InjectDictionary(this IDictionary<string, object> configuration, object destination, Action<Exception>? exceptionHandler = null)
    {
        var destProps = GetProperties(destination.GetType());
        foreach (var item in configuration.Keys)
        {
            var matchingFields = from x in destProps where x.Name == item select x;
            if (!matchingFields.Any()) continue;
            try
            {
                matchingFields.First().SetValue(destination, configuration[item]);
            }
            catch (Exception ex)
            {
                exceptionHandler?.Invoke(ex);
            }
        }
    }

    public static T? ReturnAs<T>(this object me) where T : new()
    {
        if (me == null) return default;
        T instance = new();
        me.Inject(instance);
        return instance;
    }

    public static List<T?>? ReturnAsList<T>(this IEnumerable<object> mes) where T : new()
    {
        if (mes == null) return null;
        if (!mes.Any()) return new List<T?>();
        return mes.Select(x => x.ReturnAs<T?>()).ToList();
    }

    public static Dictionary<string, object?> ProjectAsDictionary(this object me, ProjectMode mode = ProjectMode.AllButIgnored, Action<Exception>? exceptionHandler = null)
    {
        var results = new Dictionary<string, object?>();
        var myProps = GetProperties(me.GetType());
        foreach (var item in myProps)
        {
            if (item.GetCustomAttributes(typeof(IgnoreProjectAttribute), true).Any())
                continue;
            if (mode == ProjectMode.Explicit && !item.GetCustomAttributes(typeof(ProjectAttribute), true).Any())
                continue;
            
            try
            {
                results[item.Name] = item.GetValue(me);
            }
            catch (Exception ex)
            {
                exceptionHandler?.Invoke(ex);
            }
        }
        return results;
    }

    public static Dictionary<string, string> ToString(this DateTime date, string prefix, params string[] formats)
    {
        var results = new Dictionary<string, string>();
        prefix ??= string.Empty;
        foreach (var format in formats)
            results[$"{prefix}{format}"] = date.ToString(format);
        return results;
    }

    public static Dictionary<string, string> ToString(this DateTimeOffset date, string prefix, params string[] formats)
    {
        var results = new Dictionary<string, string>();
        prefix ??= string.Empty;
        foreach (var format in formats)
            results[$"{prefix}{format}"] = date.ToString(format);
        return results;
    }
}

public class DTO { }