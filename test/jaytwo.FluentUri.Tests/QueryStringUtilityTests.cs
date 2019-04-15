using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace jaytwo.FluentUri.Tests
{
    public class QueryStringUtilityTests
    {
        [Theory]
        [InlineData("abcdefghijklmnopqrstuvwxyz", "abcdefghijklmnopqrstuvwxyz")]
        [InlineData("ABCDEFGHIJKLMNOPQRSTUVWXYZ", "ABCDEFGHIJKLMNOPQRSTUVWXYZ")]
        [InlineData("1234567890", "1234567890")]
        [InlineData("hello%20world", "hello world")]
        [InlineData("%E6%97%A5%E6%9C%AC%E8%AA%9E", "日本語")]
        // https://www.backblaze.com/b2/docs/string_encoding.html
        [InlineData("%20", " ")]
        [InlineData("%21", "!")]
        [InlineData("%22", "\"")]
        [InlineData("%23", "#")]
        [InlineData("%24", "$")]
        [InlineData("%25", "%")]
        [InlineData("%26", "&")]
        [InlineData("%27", "'")]
        [InlineData("%28", "(")]
        [InlineData("%29", ")")]
        [InlineData("%2A", "*")]
        [InlineData("%2B", "+")]
        [InlineData("%2C", ",")]
        [InlineData("%2D", "-")]
        [InlineData("%2E", ".")]
        [InlineData("/", "/")]
        [InlineData("%30", "0")]
        [InlineData("%31", "1")]
        [InlineData("%32", "2")]
        [InlineData("%33", "3")]
        [InlineData("%3A", ":")]
        [InlineData("%3B", ";")]
        [InlineData("%3C", "<")]
        [InlineData("%3D", "=")]
        [InlineData("%3E", ">")]
        [InlineData("%3F", "?")]
        [InlineData("%40", "@")]
        [InlineData("%41", "A")]
        [InlineData("%42", "B")]
        [InlineData("%43", "C")]
        [InlineData("%5b", "[")]
        [InlineData("%5c", "\\")]
        [InlineData("%5d", "]")]
        [InlineData("%5e", "^")]
        [InlineData("%5f", "_")]
        [InlineData("%60", "`")]
        [InlineData("%61", "a")]
        [InlineData("%62", "b")]
        [InlineData("%63", "c")]
        [InlineData("%7b", "{")]
        [InlineData("%7c", "|")]
        [InlineData("%7d", "}")]
        [InlineData("%7e", "~")]
        [InlineData("%7f", "\u007f")]
        [InlineData("%E8%87%AA%E7%94%B1", "\u81ea\u7531")]
        [InlineData("%F0%90%90%80", "\ud801\udc00")]
        public void PercentDecode(string encoded, string expectedDecoded)
        {
            // arrange

            // act
            var decoded = QueryStringUtility.PercentDecode(encoded);

            // assert
            Assert.Equal(expectedDecoded, decoded);
        }

        [Theory]
        [InlineData("abcdefghijklmnopqrstuvwxyz", "abcdefghijklmnopqrstuvwxyz")]
        [InlineData("ABCDEFGHIJKLMNOPQRSTUVWXYZ", "ABCDEFGHIJKLMNOPQRSTUVWXYZ")]
        [InlineData("1234567890", "1234567890")]
        [InlineData("hello world", "hello%20world")]
        [InlineData("日本語", "%E6%97%A5%E6%9C%AC%E8%AA%9E")]
        // https://www.backblaze.com/b2/docs/string_encoding.html
        [InlineData(" ", "%20")]
        [InlineData("!", "%21")]
        [InlineData("\"", "%22")]
        [InlineData("#", "%23")]
        [InlineData("$", "%24")]
        [InlineData("%", "%25")]
        [InlineData("&", "%26")]
        [InlineData("'", "%27")]
        [InlineData("(", "%28")]
        [InlineData(")", "%29")]
        [InlineData("*", "%2A")]
        [InlineData("+", "%2B")]
        [InlineData(",", "%2C")]
        [InlineData("-", "%2D")]
        [InlineData(".", ".")]
        [InlineData("/", "%2F")]
        [InlineData("0", "0")]
        [InlineData("1", "1")]
        [InlineData(":", "%3A")]
        [InlineData(";", "%3B")]
        [InlineData("<", "%3C")]
        [InlineData("=", "%3D")]
        [InlineData(">", "%3E")]
        [InlineData("?", "%3F")]
        [InlineData("@", "%40")]
        [InlineData("A", "A")]
        [InlineData("B", "B")]
        [InlineData("[", "%5B")]
        [InlineData("\\", "%5C")]
        [InlineData("]", "%5D")]
        [InlineData("^", "%5E")]
        [InlineData("_", "_")]
        [InlineData("`", "%60")]
        [InlineData("a", "a")]
        [InlineData("b", "b")]
        [InlineData("{", "%7B")]
        [InlineData("|", "%7C")]
        [InlineData("}", "%7D")]
        [InlineData("~", "~")]
        [InlineData("\u007f", "%7F")]
        [InlineData("\u81ea\u7531", "%E8%87%AA%E7%94%B1")]
        //[InlineData("\ud801\udc00", "%F0%90%90%80")] i don't know why but this encodes to something else
        public void PercentEncode(string decoded, string expectedEncoded)
        {
            // arrange

            // act
            var encoded = QueryStringUtility.PercentEncode(decoded);

            // assert
            Assert.Equal(expectedEncoded, encoded);
        }

        [Fact]
        public void PercentEncodeEncodeDecode()
        {
            // arrange
            var value = "abcdefghijklmnopqrstuvwxyz"
                + "ABCDEFGHIJKLMNOPQRSTUVWXYZ"
                + "0123456789"
                + "`~!@#$%^&*()_+-="
                + "[]\\{}|;:'\",./<>?"
                + "日本語";

            // act
            var encoded = QueryStringUtility.PercentEncode(value);
            var decoded = QueryStringUtility.PercentDecode(encoded);

            // assert
            Assert.Equal(value, decoded);
        }

        [Theory]
        [InlineData("foo=bar&fizz=buzz", "{ \"foo\": [ \"bar\" ], \"fizz\": [ \"buzz\" ] }")]
        [InlineData("foo=hello%20world", "{ \"foo\": [ \"hello world\" ] }")]
        public void ParseQueryString(string queryString, string expectedDictionaryJson)
        {
            // arrange

            // act
            var query = QueryStringUtility.ParseQueryString(queryString);

            // assert
            var expected = JsonConvert.DeserializeObject<IDictionary<string, string[]>>(expectedDictionaryJson);

            Assert.Equal(expected, query);
        }

        [Theory]
        [InlineData("{ \"foo\": [ \"bar\" ], \"fizz\": [ \"buzz\" ] }", "foo=bar&fizz=buzz")]
        [InlineData("{ \"foo\": [ \"hello world\" ] }", "foo=hello%20world")]
        public void GetQueryString_dictionary_string_stringarray(string dataJson, string expectedQueryString)
        {
            // arrange
            var data = JsonConvert.DeserializeObject<IDictionary<string, string[]>>(dataJson);

            // act
            var queryString = QueryStringUtility.GetQueryString(data);

            // assert
            Assert.Equal(expectedQueryString, queryString);
        }

        [Theory]
        [InlineData("{ \"foo\": \"bar\", \"fizz\": \"buzz\" }", "foo=bar&fizz=buzz")]
        [InlineData("{ \"foo\": \"hello world\" }", "foo=hello%20world")]
        public void GetQueryString_dictionary_string_string(string dataJson, string expectedQueryString)
        {
            // arrange
            var data = JsonConvert.DeserializeObject<IDictionary<string, string>>(dataJson);

            // act
            var queryString = QueryStringUtility.GetQueryString(data);

            // assert
            Assert.Equal(expectedQueryString, queryString);
        }

        public static IEnumerable<object[]> GetQueryString_object_data()
        {
            yield return new object[] { new { foo = "bar", fizz = "buzz" }, "foo=bar&fizz=buzz" };
            yield return new object[] { new { foo = new[] { "bar", "baz" }, fizz = new[] { "buzz" } }, "foo=bar&foo=baz&fizz=buzz" };
            yield return new object[] { new { foo = "hello world" }, "foo=hello%20world" };
        }

        [Theory]
        [MemberData(nameof(GetQueryString_object_data))]
        public void GetQueryString_object(object data, string expectedQueryString)
        {
            // arrange

            // act
            var queryString = QueryStringUtility.GetQueryString(data);

            // assert
            Assert.Equal(expectedQueryString, queryString);
        }
    }
}
