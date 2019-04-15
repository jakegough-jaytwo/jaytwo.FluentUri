using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace jaytwo.FluentUri
{
    internal class QueryStringUtility
    {
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

        public static string GetQueryString(IEnumerable<KeyValuePair<string, string>> data)
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

        public static IList<KeyValuePair<string, string>> ParseQueryStringAsKeyValuePairs(string queryString)
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

        private static readonly Regex percentDecodeRegex = new Regex(@"[%](<?HEX>[0-9a-fA-F]+)", regexOptions);
        internal static string PercentDecode(string value)
        {
            return percentEncodeRegex.Replace(value, match =>
            {
                var hex = match.Groups["HEX"].Value;

                var bytes = Enumerable.Range(0, hex.Length)
                    .Where(x => x % 2 == 0)
                    .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                    .ToArray();

                return Encoding.UTF8.GetString(bytes, 0, bytes.Length);
            });
        }
    }
}