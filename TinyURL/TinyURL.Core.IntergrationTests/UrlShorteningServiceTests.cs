using System.Collections.Concurrent;
using TinyURL.Core.Interfaces;
using TinyURL.Core.Services;
using TinyURL.Infrastructure.Generators;
using TinyURL.Infrastructure.Repositories;
using Moq;
using Xunit;

namespace TinyURL.Core.IntergrationTests
{
    public class UrlShorteningServiceTests
    {
        private readonly IUrlRepository _urlRepository;
        private readonly IUrlGenerator _urlGenerator;
        private readonly Mock<IUrlShorteningSettings> _urlSettingsMock;
        private readonly IUrlShorteningService _urlShorteningService;
        private int _numberOfUsers = 1000000;

        public UrlShorteningServiceTests()
        {
            _urlRepository = new InMemoryUrlRepository();
            _urlGenerator = new Base62UrlGenerator();
            _urlSettingsMock = new Mock<IUrlShorteningSettings>();

            _urlSettingsMock.SetupGet(x => x.Schema).Returns("https");
            _urlSettingsMock.SetupGet(x => x.Host).Returns("tinyurl.com");

            _urlShorteningService = new UrlShorteningService(
                _urlRepository,
                _urlGenerator,
                _urlSettingsMock.Object);
        }

        private async Task<string[]> CreateData()
        {
            string longUrl = "http://example.com/very/long/url";

            var shortLinks = new string[_numberOfUsers];

            var tasks = new List<Task>();

            // Act
            for (int i = 0; i < _numberOfUsers; i++)
            {
                var index = i;
                tasks.Add(Task.Run(() =>
                {
                    shortLinks[index] = _urlShorteningService.CreateShortUrl(longUrl);
                }));
            }

            await Task.WhenAll(tasks);

            return shortLinks;
        }

        [Fact]
        public async Task CreateShortUrl_ShouldCreateUniqueAliases_WhenCalledConcurrently()
        {
            string[] shortLinks = await CreateData();

            // Assert
            Assert.Equal(_numberOfUsers, shortLinks.Count(l=>!string.IsNullOrEmpty(l)));
            Assert.Equal(_numberOfUsers, shortLinks.Distinct().Count());
        }

        [Fact]
        public async Task GetLongUrl_ShouldTrackClickCount_WhenCalledConcurrently()
        {
            ConcurrentDictionary<string, int> clickCounts = new ConcurrentDictionary<string, int>();

            string[] shortLinks = await CreateData();

            var tasks = new List<Task>();

            Random random = new Random();

            // Act
            for (int i = 0; i < _numberOfUsers; i++)
            {
                tasks.Add(Task.Run(() =>
                {
                    var index = random.Next(_numberOfUsers);
                    var shortUrl = shortLinks[index];

                    string resultUrl = _urlShorteningService.GetLongUrl(shortUrl);
                    clickCounts.AddOrUpdate(shortUrl, 1, (key, oldValue) => oldValue + 1);
                }));
            }

            await Task.WhenAll(tasks);

            // Assert

            foreach (var shortUrl in clickCounts)
            {
                var actual = _urlShorteningService.GetClickCount(shortUrl.Key);
                var expected = shortUrl.Value;

                Assert.Equal(expected, actual);
            }
        }

        [Fact]
        public async Task AllActions_WhenCalledConcurrently()
        {
            // Parallel tasks for multiple users
            var tasks = new List<Task>();

            string longUrl = "http://example.com/very/long/url";

            for (int i = 0; i < _numberOfUsers; i++)
            {
                tasks.Add(Task.Run(() =>
                {
                    var shortUrl = _urlShorteningService.CreateShortUrl(longUrl);
                    Assert.NotEmpty(shortUrl);

                    var retrievedLongUrl = _urlShorteningService.GetLongUrl(shortUrl);
                    Assert.Equal(longUrl, retrievedLongUrl);

                    var clickCount = _urlShorteningService.GetClickCount(shortUrl);
                    Assert.True(clickCount == 1);

                    var isDeleted = _urlShorteningService.DeleteShortUrl(shortUrl);
                    Assert.True(isDeleted);
                }));
            }

            await Task.WhenAll(tasks);
        }

    }
}
