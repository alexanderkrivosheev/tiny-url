using System;
using Xunit;
using Moq;
using TinyURL.Core.Interfaces;
using TinyURL.Core.Models;
using TinyURL.Core.Services;

namespace TinyURL.Core.Tests.Services
{
    public class UrlShorteningServiceTests
    {
        private readonly UrlShorteningService _service;
        private readonly Mock<IUrlRepository> _urlRepositoryMock;
        private readonly Mock<IUrlGenerator> _urlGeneratorMock;
        private readonly Mock<IUrlShorteningSettings> _urlSettingsMock;

        public UrlShorteningServiceTests()
        {
            _urlRepositoryMock = new Mock<IUrlRepository>();
            _urlGeneratorMock = new Mock<IUrlGenerator>();
            _urlSettingsMock = new Mock<IUrlShorteningSettings>();
            _urlSettingsMock.Setup(x => x.Schema).Returns("https");
            _urlSettingsMock.Setup(x => x.Host).Returns("short.url");

            _service = new UrlShorteningService(
                _urlRepositoryMock.Object,
                _urlGeneratorMock.Object,
                _urlSettingsMock.Object
            );
        }

        [Fact]
        public void CreateShortUrl_ShouldReturnShortUrl_WithCustomAlias()
        {
            // Arrange
            var longUrl = "https://example.com";
            var customAlias = "custom123";
            var expectedShortUrl = "https://short.url/custom123";

            // Act
            var result = _service.CreateShortUrl(longUrl, customAlias);

            // Assert
            Assert.Equal(expectedShortUrl, result);
            _urlRepositoryMock.Verify(x => x.SaveUrlMapping(It.Is<UrlMapping>(m => m.Alias == customAlias && m.ShortUrl == expectedShortUrl && m.LongUrl == longUrl)), Times.Once);
        }

        [Fact]
        public void CreateShortUrl_ShouldReturnShortUrl_WithGeneratedAlias()
        {
            // Arrange
            var longUrl = "https://example.com";
            var generatedAlias = "generated123";
            var expectedShortUrl = "https://short.url/generated123";

            _urlGeneratorMock.Setup(x => x.CreateAlias(longUrl)).Returns(generatedAlias);

            // Act
            var result = _service.CreateShortUrl(longUrl);

            // Assert
            Assert.Equal(expectedShortUrl, result);
            _urlRepositoryMock.Verify(x => x.SaveUrlMapping(It.Is<UrlMapping>(m => m.Alias == generatedAlias && m.ShortUrl == expectedShortUrl && m.LongUrl == longUrl)), Times.Once);
        }

        [Fact]
        public void DeleteShortUrl_ShouldReturnFalse_WhenAliasIsInvalid()
        {
            // Arrange
            var shortUrl = "https://short.url/invalid";
            _urlGeneratorMock.Setup(x => x.ExtractAlias(shortUrl)).Returns((string)null);

            // Act
            var result = _service.DeleteShortUrl(shortUrl);

            // Assert
            Assert.False(result);
            _urlRepositoryMock.Verify(x => x.DeleteUrlMapping(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public void DeleteShortUrl_ShouldReturnTrue_WhenAliasIsValid()
        {
            // Arrange
            var shortUrl = "https://short.url/valid";
            var alias = "valid";

            _urlGeneratorMock.Setup(x => x.ExtractAlias(shortUrl)).Returns(alias);
            _urlRepositoryMock.Setup(x => x.DeleteUrlMapping(alias)).Returns(true);

            // Act
            var result = _service.DeleteShortUrl(shortUrl);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void GetClickCount_ShouldReturnZero_WhenAliasIsInvalid()
        {
            // Arrange
            var shortUrl = "https://short.url/invalid";
            _urlGeneratorMock.Setup(x => x.ExtractAlias(shortUrl)).Returns((string)null);

            // Act
            var result = _service.GetClickCount(shortUrl);

            // Assert
            Assert.Equal(0, result);
            _urlRepositoryMock.Verify(x => x.GetUrlMapping(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public void GetClickCount_ShouldReturnClickCount_WhenAliasIsValid()
        {
            // Arrange
            var shortUrl = "https://short.url/valid";
            var alias = "valid";
            var urlMapping = new UrlMapping { Alias = alias, ClickCount = 5 };

            _urlGeneratorMock.Setup(x => x.ExtractAlias(shortUrl)).Returns(alias);
            _urlRepositoryMock.Setup(x => x.GetUrlMapping(alias)).Returns(urlMapping);

            // Act
            var result = _service.GetClickCount(shortUrl);

            // Assert
            Assert.Equal(5, result);
        }

        [Fact]
        public void GetLongUrl_ShouldReturnEmptyString_WhenUrlMappingIsNotFound()
        {
            // Arrange
            var shortUrl = "https://short.url/valid";
            var alias = "valid";

            _urlGeneratorMock.Setup(x => x.ExtractAlias(shortUrl)).Returns(alias);
            _urlRepositoryMock.Setup(x => x.GetUrlMapping(alias)).Returns((UrlMapping)null);

            // Act
            var result = _service.GetLongUrl(shortUrl);

            // Assert
            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public void GetLongUrl_ShouldReturnLongUrl_AndIncrementClickCount()
        {
            // Arrange
            var shortUrl = "https://short.url/valid";
            var alias = "valid";
            var longUrl = "https://example.com";
            var urlMapping = new UrlMapping { Alias = alias, LongUrl = longUrl, ClickCount = 5 };

            _urlGeneratorMock.Setup(x => x.ExtractAlias(shortUrl)).Returns(alias);
            _urlRepositoryMock.Setup(x => x.GetUrlMapping(alias)).Returns(urlMapping);

            // Act
            var result = _service.GetLongUrl(shortUrl);

            // Assert
            Assert.Equal(longUrl, result);
            Assert.Equal(6, urlMapping.ClickCount);
            _urlRepositoryMock.Verify(x => x.SaveUrlMapping(urlMapping), Times.Once);
        }
    }
}
