using System;
using System.Collections.Generic;
using Xunit;

namespace jaytwo.FluentUri.Tests
{
    public class UriExtensionsTests
    {
        [Theory]
        [InlineData("https://www.google.com", false)]
        [InlineData("http://www.google.com", true)]
        public void IsHttp(string baseUrl, bool expectedResult)
        {
            // arrange
            var baseUri = new Uri(baseUrl);

            // act
            var result = baseUri.IsHttp();

            // assert
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData("https://www.google.com", true)]
        [InlineData("http://www.google.com", false)]
        public void IsHttps(string baseUrl, bool expectedResult)
        {
            // arrange
            var baseUri = new Uri(baseUrl);

            // act
            var result = baseUri.IsHttps();

            // assert
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData("http://www.google.com", "www.yahoo.com", "http://www.yahoo.com")]
        [InlineData("http://www.google.com/a", "www.yahoo.com", "http://www.yahoo.com/a")]
        public void WithHost(string baseUrl, string newHost, string expectedUrl)
        {
            // arrange
            var baseUri = new Uri(baseUrl);

            // act
            var uri = baseUri.WithHost(newHost);

            // assert
            Assert.Equal(new Uri(expectedUrl), uri);
        }

        [Theory]
        [InlineData("http://www.google.com", "http://www.google.com")]
        [InlineData("https://www.google.com", "http://www.google.com")]
        public void WithHttp(string baseUrl, string expectedUrl)
        {
            // arrange
            var baseUri = new Uri(baseUrl);

            // act
            var uri = baseUri.WithHttp();

            // assert
            Assert.Equal(new Uri(expectedUrl), uri);
        }

        [Theory]
        [InlineData("http://www.google.com", "https://www.google.com")]
        [InlineData("https://www.google.com", "https://www.google.com")]
        public void WithHttps(string baseUrl, string expectedUrl)
        {
            // arrange
            var baseUri = new Uri(baseUrl);

            // act
            var uri = baseUri.WithHttps();

            // assert
            Assert.Equal(new Uri(expectedUrl), uri);
        }

        [Theory]
        [InlineData("http://www.google.com", "http://www.google.com")]
        [InlineData("http://www.google.com/a/b", "http://www.google.com")]
        [InlineData("http://www.google.com/a/b?c=d", "http://www.google.com?c=d")]
        [InlineData("http://www.google.com/a/b/?c=d", "http://www.google.com?c=d")]
        public void WithoutPath(string baseUrl, string expectedUrl)
        {
            // arrange
            var baseUri = new Uri(baseUrl);

            // act
            var uri = baseUri.WithoutPath();

            // assert
            Assert.Equal(new Uri(expectedUrl), uri);
        }

        [Theory]
        [InlineData("http://www.google.com", "http://www.google.com")]
        [InlineData("http://www.google.com/a/b", "http://www.google.com/")]
        [InlineData("http://www.google.com/a/b?c=d", "http://www.google.com/")]
        [InlineData("http://www.google.com/a/b/?c=d", "http://www.google.com/")]
        public void WithoutPathAndQuery(string baseUrl, string expectedUrl)
        {
            // arrange
            var baseUri = new Uri(baseUrl);

            // act
            var uri = baseUri.WithoutPathAndQuery();

            // assert
            Assert.Equal(new Uri(expectedUrl), uri);
        }

        [Theory]
        [InlineData("http://www.google.com:8080", "http://www.google.com")]
        [InlineData("http://www.google.com:80", "http://www.google.com")]
        [InlineData("http://www.google.com", "http://www.google.com")]
        public void WithoutPort(string baseUrl, string expectedUrl)
        {
            // arrange
            var baseUri = new Uri(baseUrl);

            // act
            var uri = baseUri.WithoutPort();

            // assert
            Assert.Equal(new Uri(expectedUrl), uri);
        }

        [Theory]
        [InlineData("http://www.google.com", "http://www.google.com")]
        [InlineData("http://www.google.com/a/b", "http://www.google.com/a/b")]
        [InlineData("http://www.google.com/a/b?c=d", "http://www.google.com/a/b")]
        [InlineData("http://www.google.com/a/b/?c=d", "http://www.google.com/a/b/")]
        [InlineData("/a/b/?c=d", "/a/b/")]
        [InlineData("../a/b/?c=d", "../a/b/")]
        public void WithoutQuery(string baseUrl, string expectedUrl)
        {
            // arrange
            Uri baseUri = new Uri(baseUrl, UriKind.RelativeOrAbsolute);

            // act
            var uri = baseUri.WithoutQuery();

            // assert
            Assert.Equal(new Uri(expectedUrl, UriKind.RelativeOrAbsolute), uri);
        }

        [Theory]
        [InlineData("http://www.google.com/?foo=bar", "foo", "http://www.google.com/")]
        [InlineData("http://www.google.com/?foo=bar&fizz=buzz", "foo", "http://www.google.com/?fizz=buzz")]
        [InlineData("http://www.google.com/?foo=bar&fizz=buzz", "fizz", "http://www.google.com/?foo=bar")]
        [InlineData("/?foo=bar&fizz=buzz", "fizz", "/?foo=bar")]
        [InlineData("../?foo=bar&fizz=buzz", "fizz", "../?foo=bar")]
        public void WithoutQueryParameter(string baseUrl, string queryParameter, string expectedUrl)
        {
            // arrange
            var baseUri = new Uri(baseUrl, UriKind.RelativeOrAbsolute);

            // act
            var uri = baseUri.WithoutQueryParameter(queryParameter);

            // assert
            Assert.Equal(new Uri(expectedUrl, UriKind.RelativeOrAbsolute), uri);
        }

        [Theory]
        [InlineData("http://www.google.com", "foo", "http://www.google.com/foo")]
        [InlineData("http://www.google.com/", "/foo", "http://www.google.com/foo")]
        [InlineData("http://www.google.com/foo", "bar", "http://www.google.com/foo/bar")]
        [InlineData("http://www.google.com/foo?hello=world", "bar", "http://www.google.com/foo/bar?hello=world")]
        [InlineData("http://www.google.com/foo", "/bar", "http://www.google.com/bar")]
        [InlineData("http://www.google.com/foo?hello=world", "/bar", "http://www.google.com/bar?hello=world")]
        [InlineData("/foo", "/bar", "/bar")]
        [InlineData("/foo", "bar", "/foo/bar")]
        [InlineData("/foo?hello=world", "/bar", "/bar?hello=world")]
        [InlineData("/foo?hello=world", "bar", "/foo/bar?hello=world")]
        [InlineData("", "bar", "bar")]
        public void WithPath(string baseUrl, string path, string expectedUrl)
        {
            // arrange
            var baseUri = new Uri(baseUrl, UriKind.RelativeOrAbsolute);

            // act
            var uri = baseUri.WithPath(path);

            // assert
            Assert.Equal(new Uri(expectedUrl, UriKind.RelativeOrAbsolute), uri);
        }

        [Theory]
        [InlineData("http://www.google.com/abc/def/ghi/?foo=bar/biz", "abc|def|ghi|")]
        [InlineData("http://www.google.com/abc/def/ghi", "abc|def|ghi")]
        [InlineData("http://www.google.com/abc//def/ghi", "abc||def|ghi")]
        public void GetPathSegments(string url, string expected)
        {
            // arrange
            var expectedPathSegments = expected.Split('|');
            var uri = new Uri(url);

            // act
            var actual = uri.GetPathSegments();

            // assert
            Assert.Equal(expectedPathSegments, actual);
        }

        [Theory]
        [InlineData("http://www.google.com/abc/def/ghi/?foo=bar/biz", 0, "abc")]
        [InlineData("http://www.google.com/abc/def/ghi/?foo=bar/biz", 1, "def")]
        [InlineData("http://www.google.com/abc/def/ghi/?foo=bar/biz", 2, "ghi")]
        [InlineData("http://www.google.com/abc/def/ghi/?foo=bar/biz", 3, "")]
        [InlineData("http://www.google.com/abc/def/ghi/?foo=bar/biz", 4, null)]
        [InlineData("http://www.google.com/abc/def/ghi/?foo=bar/biz", 99, null)]
        public void GetPathSegment(string url, int index, string expected)
        {
            // arrange
            var uri = new Uri(url);

            // act
            var actual = uri.GetPathSegment(index);

            // assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("http://www.google.com/abc/def/ghi/?foo=bar", 0, "xyz", "http://www.google.com/xyz/def/ghi/?foo=bar")]
        [InlineData("http://www.google.com/abc/def/ghi/?foo=bar", 0, "hello world", "http://www.google.com/hello%20world/def/ghi/?foo=bar")]
        [InlineData("/abc/def/ghi/?foo=bar/biz", 0, "xyz", "/xyz/def/ghi/?foo=bar/biz")]
        [InlineData("/abc/def/ghi/?foo=bar/biz", 0, "hello world", "/hello%20world/def/ghi/?foo=bar/biz")]
        [InlineData("/abc/def/ghi/?foo=bar/biz", 1, "xyz", "/abc/xyz/ghi/?foo=bar/biz")]
        [InlineData("/abc/def/ghi/?foo=bar/biz", 1, "hello world", "/abc/hello%20world/ghi/?foo=bar/biz")]
        [InlineData("../abc/def/ghi/?foo=bar/biz", 0, "xyz", "xyz/abc/def/ghi/?foo=bar/biz")]
        [InlineData("../abc/def/ghi/?foo=bar/biz", 1, "xyz", "../xyz/def/ghi/?foo=bar/biz")]
        public void WithPathSegment(string baseUrl, int index, string value, string expectedUrl)
        {
            // arrange
            var baseUri = new Uri(baseUrl, UriKind.RelativeOrAbsolute);

            // act
            var uri = baseUri.WithPathSegment(index, value);

            // assert
            Assert.Equal(new Uri(expectedUrl, UriKind.RelativeOrAbsolute), uri);
        }

        [Theory]
        [InlineData("http://www.google.com", "hello/{0}", new[] { "a b" }, "http://www.google.com/hello/a%20b")]
        [InlineData("http://www.google.com/foo", "hello/{0}", new[] { "a b" }, "http://www.google.com/foo/hello/a%20b")]
        [InlineData("/foo", "hello/{0}", new[] { "a b" }, "/foo/hello/a%20b")]
        [InlineData("/foo?x=y", "hello/{0}", new[] { "a b" }, "/foo/hello/a%20b?x=y")]
        public void WithPath_Format(string baseUrl, string pathFormat, string[] formatArgs, string expectedUrl)
        {
            // arrange
            var baseUri = new Uri(baseUrl, UriKind.RelativeOrAbsolute);

            // act
            var uri = baseUri.WithPath(pathFormat, formatArgs);

            // assert
            Assert.Equal(new Uri(expectedUrl, UriKind.RelativeOrAbsolute), uri);
        }

        [Theory]
        [InlineData("http://www.google.com", 8080, "http://www.google.com:8080")]
        [InlineData("http://www.google.com", 80, "http://www.google.com")]
        [InlineData("https://www.google.com", 8083, "https://www.google.com:8083")]
        [InlineData("https://www.google.com", 443, "https://www.google.com")]
        public void WithPort(string baseUrl, int port, string expectedUrl)
        {
            // arrange
            var baseUri = new Uri(baseUrl);

            // act
            var uri = baseUri.WithPort(port);

            // assert
            Assert.Equal(new Uri(expectedUrl), uri);
        }

        [Fact]
        public void WithQuery_anonymous_object()
        {
            // arrange
            var baseUri = new Uri("http://www.google.com/");

            // act
            var uri = baseUri.WithQuery(new { foo = "bar", fizz = "buzz" });

            // assert
            Assert.Equal(new Uri("http://www.google.com/?foo=bar&fizz=buzz"), uri);
        }

        [Fact]
        public void WithQuery_dictionary_string_string()
        {
            // arrange
            var baseUri = new Uri("http://www.google.com/");
            var data = new Dictionary<string, string>()
            {
                { "foo", "bar" },
                { "fizz", "buzz" },
                { "fi&zz", "bu zz" },
            };

            // act
            var uri = baseUri.WithQuery(data);

            // assert
            Assert.Equal(new Uri("http://www.google.com/?foo=bar&fizz=buzz&fi%26zz=bu%20zz"), uri);
        }

        [Fact]
        public void WithQuery_dictionary_string_object()
        {
            // arrange
            var baseUri = new Uri("http://www.google.com/");
            var data = new Dictionary<string, object>()
            {
                { "foo", "bar" },
                { "fizz", new[] { "buzz", "bu zz" } },
            };

            // act
            var uri = baseUri.WithQuery(data);

            // assert
            Assert.Equal(new Uri("http://www.google.com/?foo=bar&fizz=buzz&fizz=bu%20zz"), uri);
        }

        [Fact]
        public void WithQuery_dictionary_string_stringarray()
        {
            // arrange
            var baseUri = new Uri("http://www.google.com/");
            var data = new Dictionary<string, string[]>()
            {
                { "foo", new[] { "bar" } },
                { "fizz", new[] { "buzz" } },
                { "fi&zz", new[] { "bu zz" } },
            };

            // act
            var uri = baseUri.WithQuery(data);

            // assert
            Assert.Equal(new Uri("http://www.google.com/?foo=bar&fizz=buzz&fi%26zz=bu%20zz"), uri);
        }

        [Theory]
        [InlineData("http://www.google.com/?foo=bar", "fizz", "buzz", "http://www.google.com/?foo=bar&fizz=buzz")]
        [InlineData("http://www.google.com/?foo=bar", "fi&zz", "bu zz", "http://www.google.com/?foo=bar&fi%26zz=bu%20zz")]
        [InlineData("http://www.google.com/?fi%26zz=bu%20zz", "foo", "bar", "http://www.google.com/?fi%26zz=bu%20zz&foo=bar")]
        [InlineData("/a?foo=bar", "fizz", "buzz", "/a?foo=bar&fizz=buzz")]
        [InlineData("/a?foo=bar", "fi&zz", "bu zz", "/a?foo=bar&fi%26zz=bu%20zz")]
        [InlineData("/a?fi%26zz=bu%20zz", "foo", "bar", "/a?fi%26zz=bu%20zz&foo=bar")]
        [InlineData("", "foo", "bar", "?foo=bar")]
        public void WithQueryParameter_string(string baseUrl, string key, string value, string expectedUrl)
        {
            // arrange
            var baseUri = new Uri(baseUrl, UriKind.RelativeOrAbsolute);

            // act
            var uri = baseUri.WithQueryParameter(key, value);

            // assert
            Assert.Equal(new Uri(expectedUrl, UriKind.RelativeOrAbsolute), uri);
        }

        [Theory]
        [InlineData("http://www.google.com/?foo=bar", "fizz", "buzz", "bar", "http://www.google.com/?foo=bar&fizz=buzz&fizz=bar")]
        [InlineData("http://www.google.com/?foo=bar", "fi&zz", "bu zz", "bar", "http://www.google.com/?foo=bar&fi%26zz=bu%20zz&fi%26zz=bar")]
        [InlineData("http://www.google.com/?fi%26zz=bu%20zz", "foo", "bar", "baz", "http://www.google.com/?fi%26zz=bu%20zz&foo=bar&foo=baz")]
        [InlineData("/a?foo=bar", "fizz", "buzz", "baz", "/a?foo=bar&fizz=buzz&fizz=baz")]
        [InlineData("/a?foo=bar", "fi&zz", "bu zz", "baz", "/a?foo=bar&fi%26zz=bu%20zz&fi%26zz=baz")]
        [InlineData("/a?fi%26zz=bu%20zz", "foo", "bar", "baz", "/a?fi%26zz=bu%20zz&foo=bar&foo=baz")]
        [InlineData("", "foo", "bar", "baz", "?foo=bar&foo=baz")]
        public void WithQueryParameter_stringarray(string baseUrl, string key, string value1, string value2, string expectedUrl)
        {
            // arrange
            var baseUri = new Uri(baseUrl, UriKind.RelativeOrAbsolute);

            // act
            var uri = baseUri.WithQueryParameter(key, new string[] { value1, value2 });

            // assert
            Assert.Equal(new Uri(expectedUrl, UriKind.RelativeOrAbsolute), uri);
        }

        [Theory]
        [InlineData("http://www.google.com/?foo=bar", "fizz", 0, "http://www.google.com/?foo=bar&fizz=0")]
        [InlineData("http://www.google.com/?foo=bar", "fi&zz", "bu zz", "http://www.google.com/?foo=bar&fi%26zz=bu%20zz")]
        [InlineData("http://www.google.com/?fi%26zz=bu%20zz", "foo", "bar", "http://www.google.com/?fi%26zz=bu%20zz&foo=bar")]
        [InlineData("/a?foo=bar", "fizz", "buzz", "/a?foo=bar&fizz=buzz")]
        [InlineData("/a?foo=bar", "fi&zz", "bu zz", "/a?foo=bar&fi%26zz=bu%20zz")]
        [InlineData("/a?fi%26zz=bu%20zz", "foo", "bar", "/a?fi%26zz=bu%20zz&foo=bar")]
        public void WithQueryParameter_object(string baseUrl, string key, object value, string expectedUrl)
        {
            // arrange
            var baseUri = new Uri(baseUrl, UriKind.RelativeOrAbsolute);

            // act
            var uri = baseUri.WithQueryParameter(key, value);

            // assert
            Assert.Equal(new Uri(expectedUrl, UriKind.RelativeOrAbsolute), uri);
        }

        [Theory]
        [InlineData("http://www.google.com/?foo=bar", "fizz", 0, 1, "http://www.google.com/?foo=bar&fizz=0&fizz=1")]
        [InlineData("http://www.google.com/?foo=bar", "fi&zz", "bu zz", 1, "http://www.google.com/?foo=bar&fi%26zz=bu%20zz&fi%26zz=1")]
        [InlineData("http://www.google.com/?fi%26zz=bu%20zz", "foo", "bar", 1, "http://www.google.com/?fi%26zz=bu%20zz&foo=bar&foo=1")]
        [InlineData("/a?foo=bar", "fizz", "buzz", 1, "/a?foo=bar&fizz=buzz&fizz=1")]
        [InlineData("/a?foo=bar", "fi&zz", "bu zz", 1, "/a?foo=bar&fi%26zz=bu%20zz&fi%26zz=1")]
        [InlineData("/a?fi%26zz=bu%20zz", "foo", "bar", 1, "/a?fi%26zz=bu%20zz&foo=bar&foo=1")]
        public void WithQueryParameter_objectarray(string baseUrl, string key, object value1, object value2, string expectedUrl)
        {
            // arrange
            var baseUri = new Uri(baseUrl, UriKind.RelativeOrAbsolute);

            // act
            var uri = baseUri.WithQueryParameter(key, new object[] { value1, value2 });

            // assert
            Assert.Equal(new Uri(expectedUrl, UriKind.RelativeOrAbsolute), uri);
        }

        [Theory]
        [InlineData("http://www.google.com", "http", "http://www.google.com")]
        [InlineData("https://www.google.com", "http", "http://www.google.com")]
        [InlineData("http://www.google.com", "https", "https://www.google.com")]
        [InlineData("https://www.google.com", "https", "https://www.google.com")]
        public void WithScheme(string baseUrl, string scheme, string expectedUrl)
        {
            // arrange
            var baseUri = new Uri(baseUrl);

            // act
            var uri = baseUri.WithScheme(scheme);

            // assert
            Assert.Equal(new Uri(expectedUrl), uri);
        }

        [Theory]
        [InlineData("/foo", "/foo")]
        [InlineData("/foo/bar", "/foo/bar")]
        public void WithPath_without_url(string path, string expectedUrl)
        {
            // arrange
            var uri = new Uri(string.Empty, UriKind.RelativeOrAbsolute);

            // act
            uri = uri.WithPath(path);

            // assert
            Assert.Equal(expectedUrl, uri.ToString());
        }

        [Theory]
        [InlineData("foo=bar", "?foo=bar")]
        public void WithQuery_without_url(string query, string expectedUrl)
        {
            // arrange
            var uri = new Uri(string.Empty, UriKind.RelativeOrAbsolute);

            // act
            uri = uri.WithQuery(query);

            // assert
            Assert.Equal(expectedUrl, uri.ToString());
        }

        [Theory]
        [InlineData("/hello", "foo=bar", "/hello?foo=bar")]
        public void WithQuery_before_WithPath(string path, string query, string expectedUrl)
        {
            // arrange
            var uri = new Uri(string.Empty, UriKind.RelativeOrAbsolute);

            // act
            uri = uri.WithQuery(query).WithPath(path);

            // assert
            Assert.Equal(expectedUrl, uri.ToString());
        }
    }
}
