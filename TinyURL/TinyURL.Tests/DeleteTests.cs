using Xunit;
using Moq;
using TinyURL.Core.Interfaces;
using TinyURL.Commands;

namespace TinyURL.Core.Tests.Commands
{
    public class DeleteTests
    {
        private readonly Mock<IUrlShorteningService> _urlShorteningServiceMock;
        private readonly Delete _deleteCommand;

        public DeleteTests()
        {
            _urlShorteningServiceMock = new Mock<IUrlShorteningService>();
            _deleteCommand = new Delete(_urlShorteningServiceMock.Object);
        }

        [Fact]
        public async Task RunAsync_ShouldReturnFalse_WhenUrlParameterIsMissing()
        {
            // Arrange
            var args = new Dictionary<string, string>();

            // Act
            var result = await _deleteCommand.RunAsyn(args);

            // Assert
            Assert.False(result);
            _urlShorteningServiceMock.Verify(x => x.DeleteShortUrl(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task RunAsync_ShouldReturnFalse_WhenUrlParameterIsEmpty()
        {
            // Arrange
            var args = new Dictionary<string, string> { { "url", "" } };

            // Act
            var result = await _deleteCommand.RunAsyn(args);

            // Assert
            Assert.False(result);
            _urlShorteningServiceMock.Verify(x => x.DeleteShortUrl(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task RunAsync_ShouldReturnTrueAndDeleteShortUrl_WhenUrlIsValid()
        {
            // Arrange
            var shortUrl = "http://tinyurl.com/example";
            var args = new Dictionary<string, string> { { "url", shortUrl } };

            _urlShorteningServiceMock.Setup(x => x.DeleteShortUrl(shortUrl)).Returns(true);

            // Act
            var result = await _deleteCommand.RunAsyn(args);

            // Assert
            Assert.True(result);
            _urlShorteningServiceMock.Verify(x => x.DeleteShortUrl(shortUrl), Times.Once);
        }

        [Fact]
        public async Task RunAsync_ShouldReturnTrueAndShowNotFoundMessage_WhenUrlIsNotFound()
        {
            // Arrange
            var shortUrl = "http://tinyurl.com/notfound";
            var args = new Dictionary<string, string> { { "url", shortUrl } };

            _urlShorteningServiceMock.Setup(x => x.DeleteShortUrl(shortUrl)).Returns(false);

            // Act
            var result = await _deleteCommand.RunAsyn(args);

            // Assert
            Assert.True(result);
            _urlShorteningServiceMock.Verify(x => x.DeleteShortUrl(shortUrl), Times.Once);
        }
    }
}
