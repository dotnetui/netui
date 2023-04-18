using System;
using System.Text;
using System.Linq;
using System.Globalization;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Net.Essentials
{
    public static class StringExtensions
    {
        static TextInfo GetTextInfo()
        {
            try
            {
                return new CultureInfo("en-US", false).TextInfo;
            }
            catch
            {
                return CultureInfo.CurrentCulture?.TextInfo;
            }
        }

        static readonly TextInfo EnUs = GetTextInfo();

        public static string Head(this string s, int take = 20)
        {
            if (s == null) return "";
            s = s.Trim().Replace("\r", " ").Replace("\n", " ");
            if (s.Length <= take) return s;
            return string.Concat(s.Substring(0, take), "...");
        }

        public static HashSet<string> Hashtags(this string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return new HashSet<string>();
            var pattern = @"#(\w*[0-9a-zA-Z]+\w*[0-9a-zA-Z])";
            var results = new HashSet<string>();
            foreach (Match m in Regex.Matches(s, pattern))
                results.Add(m.Value.ToLowerInvariant());
            return results;
        }

        public static string RemoveDuplicateTags(string input, bool humanFormatted = false, string separator = ",")
        {
            if (string.IsNullOrWhiteSpace(input)) return "";
            var result = new HashSet<string>();
            foreach (var category in input.Split(new[] { separator }, StringSplitOptions.RemoveEmptyEntries))
            {
                result.Add(category.Trim().ToLowerInvariant());
            }
            if (humanFormatted) separator += " ";
            return string.Join(separator, result);
        }

        public static string RemovePrefix(this string s, string prefix)
        {
            if (s == null) return null;
            if (!s.StartsWith(prefix)) return s;
            var prefixLength = prefix.Length;
            return s.Substring(prefixLength);
        }

        public static DateTime DateFromEpochMs(this long epochMs)
        {
            return new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(epochMs);
        }

        public static double ToEpochMs(this DateTime dateTime)
        {
            return (dateTime - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
        }

        public static bool IsNullOrWhiteSpace(this string s) => string.IsNullOrWhiteSpace(s);
        public static bool HasValue(this string s) => !string.IsNullOrWhiteSpace(s);

        public static string CapitalizeFirstLetter(this string s)
        {
            if (String.IsNullOrWhiteSpace(s))
                return s;
            if (s.Length == 1)
                return s.ToUpper();
            return string.Concat(s.Remove(1).ToUpper(), s.Substring(1));
        }

        public static string ToFileNameHash(this string original, int maxLength = 20)
        {
            original = original.ToUpper();
            string result = "";
            for (int i = 0; i < original.Length; i++)
            {
                if (original[i] >= 'A' && original[i] <= 'Z')
                    result += original[i];
                else
                    result += '_';
            }
            if (result.Length > maxLength) result = result.Substring(0, maxLength);
            return result;
        }

        public static string Cap(this string original, int maxLength)
        {
            if (string.IsNullOrWhiteSpace(original))
                return original;

            return original.Length < maxLength ? original : original.Substring(0, maxLength);
        }

        public static string ToTitleCase(this string original)
        {
            if (string.IsNullOrWhiteSpace(original))
                return original;
            return EnUs.ToTitleCase(original.ToLower());
        }

        public static string GetDigits(this string original)
        {
            if (string.IsNullOrWhiteSpace(original))
                return original;

            return string.Join("", original.Where(x => x >= '0' && x <= '9'));
        }

        public static bool IsAllDigits(this string original)
        {
            if (string.IsNullOrWhiteSpace(original))
                return false;

            return original.All(x => x >= '0' && x <= '9');
        }

        public static bool HasDigits(this string original)
        {
            if (string.IsNullOrWhiteSpace(original))
                return false;

            return original.Any(x => x >= '0' && x <= '9');
        }

        public static string ToMD5(this string input)
        {
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                    sb.Append(hashBytes[i].ToString("X2"));
                return sb.ToString();
            }
        }

        public static string JoinIfNotEmpty(this string separator, params string[] elements)
        {
            if (elements == null)
                return null;
            if (separator == null)
                separator = "";
            return string.Join(separator, elements.Where(x => !string.IsNullOrWhiteSpace(x)));
        }
    }
}
