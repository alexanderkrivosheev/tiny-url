using Xunit;
using Moq;
using TinyURL.Core.Interfaces;
using TinyURL.Commands;

namespace TinyURL.Core.Tests.Commands
{
    public class GetTests
    {
        private readonly Mock<IUrlShorteningService> _urlShorteningServiceMock;
        private readonly Get _getCommand;

        public GetTests()
        {
            _urlShorteningServiceMock = new Mock<IUrlShorteningService>();
            _getCommand = new Get(_urlShorteningServiceMock.Object);
        }

        [Fact]
        public async Task RunAsync_ShouldReturnFalse_WhenUrlParameterIsMissing()
        {
            // Arrange
            var args = new Dictionary<string, string>();

            // Act
            var result = await _getCommand.RunAsyn(args);

            // Assert
            Assert.False(result);
            _urlShorteningServiceMock.Verify(x => x.GetLongUrl(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task RunAsync_ShouldReturnFalse_WhenUrlParameterIsEmpty()
        {
            // Arrange
            var args = new Dictionary<string, string> { { "url", "" } };

            // Act
            var result = await _getCommand.RunAsyn(args);

            // Assert
            Assert.False(result);
            _urlShorteningServiceMock.Verify(x => x.GetLongUrl(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task RunAsync_ShouldReturnFalseAndShowNotFoundMessage_WhenShortUrlNotFound()
        {
            // Arrange
            var shortUrl = "http://tinyurl.com/notfound";
            var args = new Dictionary<string, string> { { "url", shortUrl } };

            _urlShorteningServiceMock.Setup(x => x.GetLongUrl(shortUrl)).Returns(string.Empty);

            // Act
            var result = await _getCommand.RunAsyn(args);

            // Assert
            Assert.False(result);
            _urlShorteningServiceMock.Verify(x => x.GetLongUrl(shortUrl), Times.Once);
        }

        [Fact]
        public async Task RunAsync_ShouldReturnTrueAndShowLongUrl_WhenShortUrlIsFound()
        {
            // Arrange
            var shortUrl = "http://tinyurl.com/example";
            var longUrl = "http://example.com";
            var args = new Dictionary<string, string> { { "url", shortUrl } };

            _urlShorteningServiceMock.Setup(x => x.GetLongUrl(shortUrl)).Returns(longUrl);

            // Act
            var result = await _getCommand.RunAsyn(args);

            // Assert
            Assert.True(result);
            _urlShorteningServiceMock.Verify(x => x.GetLongUrl(shortUrl), Times.Once);
        }
    }
}
