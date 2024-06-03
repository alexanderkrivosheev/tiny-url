using Xunit;
using Moq;
using TinyURL.Core.Interfaces;
using TinyURL.Commands;

namespace TinyURL.Core.Tests.Commands
{
    public class CreateTests
    {
        private readonly Mock<IUrlShorteningService> _urlShorteningServiceMock;
        private readonly Create _createCommand;

        public CreateTests()
        {
            _urlShorteningServiceMock = new Mock<IUrlShorteningService>();
            _createCommand = new Create(_urlShorteningServiceMock.Object);
        }

        [Fact]
        public async Task RunAsync_ShouldReturnFalse_WhenUrlParameterIsMissing()
        {
            // Arrange
            var args = new Dictionary<string, string>();

            // Act
            var result = await _createCommand.RunAsyn(args);

            // Assert
            Assert.False(result);
            _urlShorteningServiceMock.Verify(x => x.CreateShortUrl(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task RunAsync_ShouldReturnFalse_WhenUrlParameterIsEmpty()
        {
            // Arrange
            var args = new Dictionary<string, string> { { "url", "" } };

            // Act
            var result = await _createCommand.RunAsyn(args);

            // Assert
            Assert.False(result);
            _urlShorteningServiceMock.Verify(x => x.CreateShortUrl(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task RunAsync_ShouldReturnFalse_WhenAliasParameterIsEmpty()
        {
            // Arrange
            var args = new Dictionary<string, string> { { "url", "http://example.com" }, { "alias", "" } };

            // Act
            var result = await _createCommand.RunAsyn(args);

            // Assert
            Assert.False(result);
            _urlShorteningServiceMock.Verify(x => x.CreateShortUrl(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task RunAsync_ShouldCreateShortUrl_WhenUrlIsProvided()
        {
            // Arrange
            var args = new Dictionary<string, string> { { "url", "http://example.com" } };
            var expectedShortUrl = "http://short.url/abc123";

            _urlShorteningServiceMock.Setup(x => x.CreateShortUrl("http://example.com", null)).Returns(expectedShortUrl);

            // Act
            var result = await _createCommand.RunAsyn(args);

            // Assert
            Assert.True(result);
            _urlShorteningServiceMock.Verify(x => x.CreateShortUrl("http://example.com", null), Times.Once);
        }

        [Fact]
        public async Task RunAsync_ShouldCreateShortUrlWithAlias_WhenUrlAndAliasAreProvided()
        {
            // Arrange
            var args = new Dictionary<string, string> { { "url", "http://example.com" }, { "alias", "example" } };
            var expectedShortUrl = "http://short.url/example";

            _urlShorteningServiceMock.Setup(x => x.CreateShortUrl("http://example.com", "example")).Returns(expectedShortUrl);

            // Act
            var result = await _createCommand.RunAsyn(args);

            // Assert
            Assert.True(result);
            _urlShorteningServiceMock.Verify(x => x.CreateShortUrl("http://example.com", "example"), Times.Once);
        }
    }
}
