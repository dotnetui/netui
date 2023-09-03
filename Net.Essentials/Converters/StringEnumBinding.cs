using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Net.Essentials.Converters
{
    public interface IStringEnumConverter
    {
        string GetString(object value);
        object GetValue(string value);
    }

    public class StringEnumConverter<T> : IStringEnumConverter where T : struct
    {
        public Dictionary<string, T> StringToEnum = new Dictionary<string, T>();
        public Dictionary<T, string> EnumToString = new Dictionary<T, string>();
        public T DefaultValue = default;
        public string DefaultString = null;

        public bool ParseStringFirst = true;
        public bool ToStringFallback = true;
        public bool DefaultValueToDefaultString = true;
        public bool ToLower = true;

        protected virtual Dictionary<string, T> Populate()
        {
            return PopulateWithSnakeCase();
        }

        public StringEnumConverter()
        {
            Populate();
            EnumToString = StringToEnum.ToDictionary(x => x.Value, x => x.Key);
        }

        protected Dictionary<string, T> PopulateWithSnakeCase()
        {
            var values = Enum.GetValues(typeof(T)).Cast<T>().ToList();
            return values.ToDictionary(x => x.ToString().ToSnakeCase(), x => x);
        }

        public virtual string GetString(T value)
        {
            string result = default;
            if (DefaultValueToDefaultString && value.Equals(DefaultValue))
                result = DefaultString;
            else if (EnumToString.TryGetValue(value, out var s))
                result = s;
            else if (ToStringFallback)
                result = value.ToString();
            if (ToLower) result = result?.ToLower();
            return result;
        }

        public virtual T GetValue(string s)
        {
            if (ParseStringFirst && Enum.TryParse<T>(s, true, out var result))
                return result;
            if (StringToEnum.TryGetValue(s, out result))
                return result;
            return DefaultValue;
        }

        public string GetString(object value)
        {
            return GetString((T)value);
        }

        object IStringEnumConverter.GetValue(string value)
        {
            return GetValue(value);
        }
    }

    public class StringEnumConverterRepository
    {
        public Dictionary<Type, IStringEnumConverter> Converters = new Dictionary<Type, IStringEnumConverter>();
        
        public static StringEnumConverterRepository Default { get; set; } = new StringEnumConverterRepository();

        public T GetValue<T>(string value) where T : struct
        {
            var type = typeof(T);
            if (!Converters.TryGetValue(type, out var binding))
                Converters[type] = binding = new StringEnumConverter<T>();
            return (T)binding.GetValue(value);
        }

        public string GetString<T>(T value) where T : struct
        {
            var type = typeof(T);
            if (!Converters.TryGetValue(type, out var binding))
                Converters[type] = binding = new StringEnumConverter<T>();
            return binding.GetString(value);
        }
    }
}
