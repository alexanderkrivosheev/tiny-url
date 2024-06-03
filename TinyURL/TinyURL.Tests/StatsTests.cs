using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Moq;
using TinyURL.Core.Interfaces;
using TinyURL.Commands;

namespace TinyURL.Core.Tests.Commands
{
    public class StatsTests
    {
        private readonly Mock<IUrlShorteningService> _urlShorteningServiceMock;
        private readonly Stats _statsCommand;

        public StatsTests()
        {
            _urlShorteningServiceMock = new Mock<IUrlShorteningService>();
            _statsCommand = new Stats(_urlShorteningServiceMock.Object);
        }

        [Fact]
        public async Task RunAsync_ShouldReturnFalse_WhenUrlParameterIsMissing()
        {
            // Arrange
            var args = new Dictionary<string, string>();

            // Act
            var result = await _statsCommand.RunAsyn(args);

            // Assert
            Assert.False(result);
            _urlShorteningServiceMock.Verify(x => x.GetClickCount(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task RunAsync_ShouldReturnFalse_WhenUrlParameterIsEmpty()
        {
            // Arrange
            var args = new Dictionary<string, string> { { "url", "" } };

            // Act
            var result = await _statsCommand.RunAsyn(args);

            // Assert
            Assert.False(result);
            _urlShorteningServiceMock.Verify(x => x.GetClickCount(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task RunAsync_ShouldReturnTrueAndShowClickCount_WhenUrlIsValid()
        {
            // Arrange
            var shortUrl = "http://tinyurl.com/example";
            var args = new Dictionary<string, string> { { "url", shortUrl } };
            var clickCount = 10;

            _urlShorteningServiceMock.Setup(x => x.GetClickCount(shortUrl)).Returns(clickCount);

            // Act
            var result = await _statsCommand.RunAsyn(args);

            // Assert
            Assert.True(result);
            _urlShorteningServiceMock.Verify(x => x.GetClickCount(shortUrl), Times.Once);
        }
    }
}
