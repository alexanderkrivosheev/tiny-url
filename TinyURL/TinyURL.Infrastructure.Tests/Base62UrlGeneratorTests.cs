using Xunit;
using TinyURL.Infrastructure.Generators;

namespace TinyURL.Core.Tests.Services
{
    public class Base62UrlGeneratorTests
    {
        private readonly Base62UrlGenerator _urlGenerator;

        public Base62UrlGeneratorTests()
        {
            _urlGenerator = new Base62UrlGenerator();
        }

        [Fact]
        public void CreateAlias_ShouldGenerateUniqueAlias_ForEachCall()
        {
            // Arrange
            var url1 = "https://example.com/1";
            var url2 = "https://example.com/2";

            // Act
            var alias1 = _urlGenerator.CreateAlias(url1);
            var alias2 = _urlGenerator.CreateAlias(url2);

            // Assert
            Assert.NotEqual(alias1, alias2);
        }

        [Fact]
        public void CreateAlias_ShouldGenerateNonEmptyAlias()
        {
            // Arrange
            var url = "https://example.com";

            // Act
            var alias = _urlGenerator.CreateAlias(url);

            // Assert
            Assert.False(string.IsNullOrEmpty(alias));
        }

        [Fact]
        public void ExtractAlias_ShouldReturnAliasFromValidShortUrl()
        {
            // Arrange
            var shortUrl = "https://short.url/abc123";

            // Act
            var alias = _urlGenerator.ExtractAlias(shortUrl);

            // Assert
            Assert.Equal("abc123", alias);
        }

        [Fact]
        public void ExtractAlias_ShouldReturnEmptyStringForInvalidShortUrl()
        {
            // Arrange
            var shortUrl = "invalid-url";

            // Act
            var alias = _urlGenerator.ExtractAlias(shortUrl);

            // Assert
            Assert.Equal("invalid-url", alias);
        }

        [Fact]
        public void ExtractAlias_ShouldReturnEmptyStringForEmptyShortUrl()
        {
            // Arrange
            var shortUrl = "";

            // Act
            var alias = _urlGenerator.ExtractAlias(shortUrl);

            // Assert
            Assert.Equal("", alias);
        }

        [Fact]
        public void ExtractAlias_ShouldReturnAliasWithNoSlashes()
        {
            // Arrange
            var shortUrl = "https://short.url/abc123/";

            // Act
            var alias = _urlGenerator.ExtractAlias(shortUrl);

            // Assert
            Assert.Equal("abc123", alias);
        }
    }
}
