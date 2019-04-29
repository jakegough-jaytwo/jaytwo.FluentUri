using System;
using System.Collections.Generic;
using Xunit;

namespace jaytwo.FluentUri.Tests
{
    public class UriExtensions
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
        public void WithoutQuery(string baseUrl, string expectedUrl)
        {
            // arrange
            var baseUri = new Uri(baseUrl);

            // act
            var uri = baseUri.WithoutQuery();

            // assert
            Assert.Equal(new Uri(expectedUrl), uri);
        }

        [Theory]
        [InlineData("http://www.google.com/?foo=bar", "foo", "http://www.google.com")]
        [InlineData("http://www.google.com/?foo=bar&fizz=buzz", "foo", "http://www.google.com?fizz=buzz")]
        [InlineData("http://www.google.com/?foo=bar&fizz=buzz", "fizz", "http://www.google.com?foo=bar")]
        public void WithoutQueryParameter(string baseUrl, string queryParameter, string expectedUrl)
        {
            // arrange
            var baseUri = new Uri(baseUrl);

            // act
            var uri = baseUri.WithoutQueryParameter(queryParameter);

            // assert
            Assert.Equal(new Uri(expectedUrl), uri);
        }

        [Theory]
        [InlineData("http://www.google.com", "foo", "http://www.google.com/foo")]
        [InlineData("http://www.google.com/", "/foo", "http://www.google.com/foo")]
        [InlineData("http://www.google.com/foo", "bar", "http://www.google.com/foo/bar")]
        [InlineData("http://www.google.com/foo?hello=world", "bar", "http://www.google.com/foo/bar?hello=world")]
        [InlineData("http://www.google.com/foo", "/bar", "http://www.google.com/bar")]
        [InlineData("http://www.google.com/foo?hello=world", "/bar", "http://www.google.com/bar?hello=world")]
        public void WithPath(string baseUrl, string path, string expectedUrl)
        {
            // arrange
            var baseUri = new Uri(baseUrl);

            // act
            var uri = baseUri.WithPath(path);

            // assert
            Assert.Equal(new Uri(expectedUrl), uri);
        }

        [Theory]
        [InlineData("http://www.google.com", "hello/{0}", new[] { "a b" }, "http://www.google.com/hello/a%20b")]
        public void WithPath_Format(string baseUrl, string pathFormat, string[] formatArgs, string expectedUrl)
        {
            // arrange
            var baseUri = new Uri(baseUrl);

            // act
            var uri = baseUri.WithPath(pathFormat, formatArgs);

            // assert
            Assert.Equal(new Uri(expectedUrl), uri);
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
                { "fi&zz", new[] {"bu zz" } },
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
        public void WithQueryParameter(string baseUrl, string key, string value, string expectedUrl)
        {
            // arrange
            var baseUri = new Uri(baseUrl);

            // act
            var uri = baseUri.WithQueryParameter(key, value);

            // assert
            Assert.Equal(new Uri(expectedUrl), uri);
        }

        [Theory]
        [InlineData("http://www.google.com/?foo=bar", "fizz", 0, "http://www.google.com/?foo=bar&fizz=0")]
        [InlineData("http://www.google.com/?foo=bar", "fi&zz", "bu zz", "http://www.google.com/?foo=bar&fi%26zz=bu%20zz")]
        [InlineData("http://www.google.com/?fi%26zz=bu%20zz", "foo", "bar", "http://www.google.com/?fi%26zz=bu%20zz&foo=bar")]
        public void WithQueryParameter_value_object(string baseUrl, string key, object value, string expectedUrl)
        {
            // arrange
            var baseUri = new Uri(baseUrl);

            // act
            var uri = baseUri.WithQueryParameter(key, value);

            // assert
            Assert.Equal(new Uri(expectedUrl), uri);
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
    }
}
