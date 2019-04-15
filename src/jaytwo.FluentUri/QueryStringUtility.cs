using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace jaytwo.FluentUri
{
    internal class QueryStringUtility
    {
        public static string GetQueryString(object data)
        {
            var runtimeProperties = data.GetType().GetRuntimeProperties();
            var dictionary = runtimeProperties
                .Where(x => x.GetValue(data) != null)
                .ToDictionary(m => m.Name, m => m.GetValue(data));

            return GetQueryString(dictionary);
        }

        public static string GetQueryString(IDictionary<string, object> data)
        {
            var asKeyValuePairs = new List<KeyValuePair<string, string>>();

            foreach (var keyValuePair in data)
            {
                var asArray = keyValuePair.Value as Array;

                if (asArray != null)
                {
                    foreach (var item in asArray)
                    {
                        asKeyValuePairs.Add(new KeyValuePair<string, string>(keyValuePair.Key, $"{item}"));
                    }
                }
                else
                {
                    asKeyValuePairs.Add(new KeyValuePair<string, string>(keyValuePair.Key, $"{keyValuePair.Value}"));
                }
            }

            return GetQueryString(asKeyValuePairs);
        }

        public static string GetQueryString(IDictionary<string, string[]> data)
        {
            var asKeyValuePairs = data.SelectMany(keyValuePair =>
                keyValuePair.Value.Select(value =>
                    new KeyValuePair<string, string>(keyValuePair.Key, value)));

            return GetQueryString(asKeyValuePairs);
        }

        public static string GetQueryString(IDictionary<string, string> data)
        {
            var asKeyValuePairs = data.ToList();
            return GetQueryString(asKeyValuePairs);
        }

        private static string GetQueryString(IEnumerable<KeyValuePair<string, string>> data)
        {
            return string.Join("&", data.Select(x => $"{PercentEncode(x.Key)}={PercentEncode(x.Value)}"));
        }

        public static IDictionary<string, string[]> ParseQueryString(string queryString)
        {
            var keyValuePairsWithDuplicateKeys = ParseQueryStringAsKeyValuePairs(queryString);

            var result = keyValuePairsWithDuplicateKeys
                .GroupBy(x => x.Key)
                .Select(group =>
                {
                    var values = group.Select(x => x.Value).ToArray();
                    return new { group.Key, values };
                })
                .ToDictionary(x => x.Key, x => x.values);

            return result;
        }

        private static IList<KeyValuePair<string, string>> ParseQueryStringAsKeyValuePairs(string queryString)
        {
            var result = new List<KeyValuePair<string, string>>();

            if (!string.IsNullOrWhiteSpace(queryString))
            {
                queryString = queryString.TrimStart('?');

                var keyValuePairs = queryString.Split('&');
                foreach (var keyValuePair in keyValuePairs)
                {
                    var keyValueSplit = keyValuePair.Split('=');
                    var key = PercentDecode(keyValueSplit[0]);
                    var value = (keyValueSplit.Length > 1) ? PercentDecode(keyValueSplit[1]) : null;
                    result.Add(new KeyValuePair<string, string>(key, value));
                }
            }

            return result;
        }

#if NETSTANDARD1_1
        private static RegexOptions regexOptions = RegexOptions.CultureInvariant;
#else
        private static RegexOptions regexOptions = RegexOptions.Compiled | RegexOptions.CultureInvariant;
#endif

        private static readonly Regex percentEncodeRegex = new Regex(@"[^A-Za-z0-9_.~]", regexOptions);
        public static string PercentEncode(string value)
        {
            return percentEncodeRegex.Replace(value, match =>
            {
                var utf8bytes = Encoding.UTF8.GetBytes(match.Value);

                var result = new StringBuilder();
                foreach (var b in utf8bytes)
                {
                    result.AppendFormat("%{0:X2}", b);
                }

                return result.ToString();
            });
        }

        private static readonly Regex percentDecodeRegex = new Regex(@"([%][0-9a-fA-F]{2})+", regexOptions);
        internal static string PercentDecode(string value)
        {
            return percentDecodeRegex.Replace(value, match =>
            {
                var hex = match.Value.Replace("%", string.Empty);

                var bytes = Enumerable.Range(0, hex.Length)
                    .Where(x => x % 2 == 0)
                    .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                    .ToArray();

                return Encoding.UTF8.GetString(bytes, 0, bytes.Length);
            });
        }
    }
}