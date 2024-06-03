using System.Collections.Concurrent;
using Xunit;
using TinyURL.Core.Interfaces;
using TinyURL.Core.Models;
using TinyURL.Infrastructure.Repositories;

namespace TinyURL.Core.Tests.Infrastructure.Repositories
{
    public class InMemoryUrlRepositoryTests
    {
        private readonly InMemoryUrlRepository _repository;

        public InMemoryUrlRepositoryTests()
        {
            _repository = new InMemoryUrlRepository();
        }

        [Fact]
        public void SaveUrlMapping_ShouldSaveMapping()
        {
            // Arrange
            var urlMapping = new UrlMapping
            {
                Alias = "example",
                ShortUrl = "http://short.url/example",
                LongUrl = "http://example.com",
                ClickCount = 0
            };

            // Act
            _repository.SaveUrlMapping(urlMapping);
            var retrievedMapping = _repository.GetUrlMapping("example");

            // Assert
            Assert.NotNull(retrievedMapping);
            Assert.Equal("example", retrievedMapping.Alias);
            Assert.Equal("http://short.url/example", retrievedMapping.ShortUrl);
            Assert.Equal("http://example.com", retrievedMapping.LongUrl);
            Assert.Equal(0, retrievedMapping.ClickCount);
        }

        [Fact]
        public void GetUrlMapping_ShouldReturnNull_WhenAliasDoesNotExist()
        {
            // Act
            var retrievedMapping = _repository.GetUrlMapping("nonexistent");

            // Assert
            Assert.Null(retrievedMapping);
        }

        [Fact]
        public void DeleteUrlMapping_ShouldReturnTrue_WhenAliasExists()
        {
            // Arrange
            var urlMapping = new UrlMapping
            {
                Alias = "example",
                ShortUrl = "http://short.url/example",
                LongUrl = "http://example.com",
                ClickCount = 0
            };

            _repository.SaveUrlMapping(urlMapping);

            // Act
            var result = _repository.DeleteUrlMapping("example");
            var retrievedMapping = _repository.GetUrlMapping("example");

            // Assert
            Assert.True(result);
            Assert.Null(retrievedMapping);
        }

        [Fact]
        public void DeleteUrlMapping_ShouldReturnFalse_WhenAliasDoesNotExist()
        {
            // Act
            var result = _repository.DeleteUrlMapping("nonexistent");

            // Assert
            Assert.False(result);
        }
    }
}
