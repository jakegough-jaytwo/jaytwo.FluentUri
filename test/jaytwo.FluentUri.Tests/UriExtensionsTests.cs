using System;
using Xunit;

namespace jaytwo.FluentUri.Tests
{
    public class UriExtensions
    {
        [Fact]
        public void WithHost()
        {
            // arrange
            var baseUri = new Uri("http://www.google.com");

            // act
            var uri = baseUri.WithHost("www.yahoo.com");

            // assert
            Assert.Equal(new Uri("http://www.yahoo.com"), uri);
        }

        [Fact]
        public void WithHttp()
        {
            // arrange
            var baseUri = new Uri("https://www.google.com");

            // act
            var uri = baseUri.WithHttp();

            // assert
            Assert.Equal(new Uri("http://www.google.com"), uri);
        }

        [Fact]
        public void WithHttps()
        {
            // arrange
            var baseUri = new Uri("http://www.google.com");

            // act
            var uri = baseUri.WithHttps();

            // assert
            Assert.Equal(new Uri("https://www.google.com"), uri);
        }

        [Fact]
        public void WithoutPath()
        {
            // arrange
            var baseUri = new Uri("http://www.google.com/a/b/c");

            // act
            var uri = baseUri.WithoutPath();

            // assert
            Assert.Equal(new Uri("http://www.google.com"), uri);
        }

        [Fact]
        public void WithoutPort()
        {
            // arrange
            var baseUri = new Uri("http://www.google.com:8080");

            // act
            var uri = baseUri.WithoutPort();

            // assert
            Assert.Equal(new Uri("http://www.google.com"), uri);
        }

        [Fact]
        public void WithoutQuery()
        {
            // arrange
            var baseUri = new Uri("http://www.google.com/hello?foo=bar");

            // act
            var uri = baseUri.WithoutQuery();

            // assert
            Assert.Equal(new Uri("http://www.google.com/hello"), uri);
        }

        [Fact]
        public void WithoutQueryParameter()
        {
            // arrange
            var baseUri = new Uri("http://www.google.com/hello?foo=bar&fizz=buzz");

            // act
            var uri = baseUri.WithoutQueryParameter("foo");

            // assert
            Assert.Equal(new Uri("http://www.google.com/hello?fizz=buzz"), uri);
        }

        [Fact]
        public void WithPath()
        {
            // arrange
            var baseUri = new Uri("http://www.google.com/");

            // act
            var uri = baseUri.WithPath("hello");

            // assert
            Assert.Equal(new Uri("http://www.google.com/hello"), uri);
        }

        [Fact]
        public void WithPath_Format()
        {
            // arrange
            var baseUri = new Uri("http://www.google.com/");

            // act
            var uri = baseUri.WithPath("hello/{0}", "a b");

            // assert
            Assert.Equal(new Uri("http://www.google.com/hello/a%20b"), uri);
        }

        [Fact]
        public void WithPort()
        {
            // arrange
            var baseUri = new Uri("http://www.google.com/");

            // act
            var uri = baseUri.WithPort(8080);

            // assert
            Assert.Equal(new Uri("http://www.google.com:8080/"), uri);
        }

        [Fact]
        public void WithQuery()
        {
            // arrange
            var baseUri = new Uri("http://www.google.com/");

            // act
            var uri = baseUri.WithQuery(new { foo = "bar", fizz = "buzz" });

            // assert
            Assert.Equal(new Uri("http://www.google.com/?foo=bar&fizz=buzz"), uri);
        }

        [Fact]
        public void WithQueryParameter()
        {
            // arrange
            var baseUri = new Uri("http://www.google.com/?foo=bar");

            // act
            var uri = baseUri.WithQueryParameter("fizz", "buzz");

            // assert
            Assert.Equal(new Uri("http://www.google.com/?foo=bar&fizz=buzz"), uri);
        }

        [Fact]
        public void WithScheme()
        {
            // arrange
            var baseUri = new Uri("http://www.google.com");

            // act
            var uri = baseUri.WithScheme("https");

            // assert
            Assert.Equal("https", uri.Scheme);
        }
    }
}
