using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace jaytwo.FluentUri.Tests
{
    public class UrlHelperTests
    {
        [Theory]
        [InlineData("http://www.google.com", "http://www.google.com")]
        [InlineData("http://www.google.com/a/b", "http://www.google.com/a/b")]
        [InlineData("http://www.google.com/a/b?c=d", "http://www.google.com/a/b")]
        [InlineData("http://www.google.com/a/b/?c=d", "http://www.google.com/a/b/")]
        [InlineData("/a/b/?c=d", "/a/b/")]
        [InlineData("../a/b/?c=d", "../a/b/")]
        public void RemoveQuery(string baseUrl, string expectedUrl)
        {
            // arrange

            // act
            var url = UrlHelper.RemoveQuery(baseUrl);

            // assert
            Assert.Equal(expectedUrl, url);
        }

        [Theory]
        [InlineData("http://www.google.com/?foo=bar", "foo", "http://www.google.com/")]
        [InlineData("http://www.google.com/?foo=bar&fizz=buzz", "foo", "http://www.google.com/?fizz=buzz")]
        [InlineData("http://www.google.com/?foo=bar&fizz=buzz", "fizz", "http://www.google.com/?foo=bar")]
        [InlineData("/?foo=bar&fizz=buzz", "fizz", "/?foo=bar")]
        [InlineData("../?foo=bar&fizz=buzz", "fizz", "../?foo=bar")]
        public void RemoveQueryParameter(string baseUrl, string queryParameter, string expectedUrl)
        {
            // arrange

            // act
            var url = UrlHelper.RemoveQueryParameter(baseUrl, queryParameter);

            // assert
            Assert.Equal(expectedUrl, url);
        }

        [Theory]
        [InlineData("http://www.google.com", "foo", "http://www.google.com/foo")]
        [InlineData("http://www.google.com/", "/foo", "http://www.google.com/foo")]
        [InlineData("http://www.google.com/foo", "bar", "http://www.google.com/bar")]
        [InlineData("http://www.google.com/foo?hello=world", "bar", "http://www.google.com/bar?hello=world")]
        [InlineData("http://www.google.com/foo", "/bar", "http://www.google.com/bar")]
        [InlineData("http://www.google.com/foo?hello=world", "/bar", "http://www.google.com/bar?hello=world")]
        [InlineData("/foo", "/bar", "/bar")]
        [InlineData("foo", "/bar", "/bar")]
        [InlineData("/foo", "bar", "/bar")]
        [InlineData("foo", "bar", "bar")]
        [InlineData("/foo?hello=world", "/bar", "/bar?hello=world")]
        [InlineData("/foo?hello=world", "bar", "/bar?hello=world")]
        public void SetPath(string baseUrl, string path, string expectedUrl)
        {
            // arrange

            // act
            var url = UrlHelper.SetPath(baseUrl, path);

            // assert
            Assert.Equal(expectedUrl, url);
        }

        [Theory]
        [InlineData("http://www.google.com", "hello/{0}", new[] { "a b" }, "http://www.google.com/hello/a%20b")]
        [InlineData("http://www.google.com/foo", "hello/{0}", new[] { "a b" }, "http://www.google.com/hello/a%20b")]
        [InlineData("/foo", "hello/{0}", new[] { "a b" }, "/hello/a%20b")]
        [InlineData("foo", "hello/{0}", new[] { "a b" }, "hello/a%20b")]
        [InlineData("/foo?x=y", "hello/{0}", new[] { "a b" }, "/hello/a%20b?x=y")]
        [InlineData("foo?x=y", "hello/{0}", new[] { "a b" }, "hello/a%20b?x=y")]
        public void SetPath_Format(string baseUrl, string pathFormat, string[] formatArgs, string expectedUrl)
        {
            // arrange

            // act
            var url = UrlHelper.SetPath(baseUrl, pathFormat, formatArgs);

            // assert
            Assert.Equal(expectedUrl, url);
        }

        [Theory]
        [InlineData("http://www.google.com", "foo", "http://www.google.com/foo")]
        [InlineData("http://www.google.com/", "/foo", "http://www.google.com/foo")]
        [InlineData("http://www.google.com/foo", "bar", "http://www.google.com/foo/bar")]
        [InlineData("http://www.google.com/foo?hello=world", "bar", "http://www.google.com/foo/bar?hello=world")]
        [InlineData("http://www.google.com/foo", "/bar", "http://www.google.com/foo/bar")]
        [InlineData("http://www.google.com/foo?hello=world", "/bar", "http://www.google.com/foo/bar?hello=world")]
        [InlineData("/foo", "/bar", "/foo/bar")]
        [InlineData("/foo", "bar", "/foo/bar")]
        [InlineData("/foo?hello=world", "/bar", "/foo/bar?hello=world")]
        [InlineData("/foo?hello=world", "bar", "/foo/bar?hello=world")]
        public void AppendPath(string baseUrl, string path, string expectedUrl)
        {
            // arrange

            // act
            var url = UrlHelper.AppendPath(baseUrl, path);

            // assert
            Assert.Equal(expectedUrl, url);
        }

        [Theory]
        [InlineData("http://www.google.com", "hello/{0}", new[] { "a b" }, "http://www.google.com/hello/a%20b")]
        [InlineData("http://www.google.com/foo", "hello/{0}", new[] { "a b" }, "http://www.google.com/foo/hello/a%20b")]
        [InlineData("/foo", "hello/{0}", new[] { "a b" }, "/foo/hello/a%20b")]
        [InlineData("/foo?x=y", "hello/{0}", new[] { "a b" }, "/foo/hello/a%20b?x=y")]
        public void AppendPath_Format(string baseUrl, string pathFormat, string[] formatArgs, string expectedUrl)
        {
            // arrange

            // act
            var url = UrlHelper.AppendPath(baseUrl, pathFormat, formatArgs);

            // assert
            Assert.Equal(expectedUrl, url);
        }

        [Theory]
        [InlineData("http://www.google.com/?foo=bar", "fizz", "buzz", "http://www.google.com/?foo=bar&fizz=buzz")]
        [InlineData("http://www.google.com/?foo=bar", "fi&zz", "bu zz", "http://www.google.com/?foo=bar&fi%26zz=bu%20zz")]
        [InlineData("http://www.google.com/?fi%26zz=bu%20zz", "foo", "bar", "http://www.google.com/?fi%26zz=bu%20zz&foo=bar")]
        [InlineData("/a?foo=bar", "fizz", "buzz", "/a?foo=bar&fizz=buzz")]
        [InlineData("/a?foo=bar", "fi&zz", "bu zz", "/a?foo=bar&fi%26zz=bu%20zz")]
        [InlineData("/a?fi%26zz=bu%20zz", "foo", "bar", "/a?fi%26zz=bu%20zz&foo=bar")]
        public void SetQueryParameter(string baseUrl, string key, string value, string expectedUrl)
        {
            // arrange

            // act
            var uri = UrlHelper.SetQueryParameter(baseUrl, key, value);

            // assert
            Assert.Equal(expectedUrl, uri);
        }

        [Theory]
        [InlineData("http://www.google.com/?foo=bar", "fizz", 0, "http://www.google.com/?foo=bar&fizz=0")]
        [InlineData("http://www.google.com/?foo=bar", "fi&zz", "bu zz", "http://www.google.com/?foo=bar&fi%26zz=bu%20zz")]
        [InlineData("http://www.google.com/?fi%26zz=bu%20zz", "foo", "bar", "http://www.google.com/?fi%26zz=bu%20zz&foo=bar")]
        [InlineData("/a?foo=bar", "fizz", "buzz", "/a?foo=bar&fizz=buzz")]
        [InlineData("/a?foo=bar", "fi&zz", "bu zz", "/a?foo=bar&fi%26zz=bu%20zz")]
        [InlineData("/a?fi%26zz=bu%20zz", "foo", "bar", "/a?fi%26zz=bu%20zz&foo=bar")]
        public void SetQueryParameter_value_object(string baseUrl, string key, object value, string expectedUrl)
        {
            // arrange

            // act
            var uri = UrlHelper.SetQueryParameter(baseUrl, key, value);

            // assert
            Assert.Equal(expectedUrl, uri);
        }
    }
}