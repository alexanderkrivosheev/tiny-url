using TinyURL.Core.Interfaces;
using TinyURL.Core.Models;

namespace TinyURL.Core.Services
{
    public class UrlShorteningService : IUrlShorteningService
    {
        private IUrlRepository _urlRepository;
        private IUrlGenerator _urlGenerator;
        private IUrlShorteningSettings _urlSettings;
        private object _lockObject = new object();
        public UrlShorteningService(
            IUrlRepository urlRepository,
            IUrlGenerator urlGenerator,
            IUrlShorteningSettings urlSettings)
        {
            _urlGenerator = urlGenerator;
            _urlRepository = urlRepository;
            _urlSettings = urlSettings;
            
        }

        public string CreateShortUrl(string longUrl, string customAlias = null)
        {
            string alias = customAlias ?? _urlGenerator.CreateAlias(longUrl);

            var uriBuilder = new UriBuilder
            {
                Scheme = _urlSettings.Schema,
                Host = _urlSettings.Host,
                Path = alias
            };

            var shortUrl = uriBuilder.Uri.ToString();

            var urlMapping = new UrlMapping { Alias = alias, ShortUrl = shortUrl, LongUrl = longUrl, ClickCount = 0 };

            _urlRepository.SaveUrlMapping(urlMapping);

            return shortUrl;
        }

        private bool ValidateShortUrl(string shortUrl, out string alias)
        {
            alias = _urlGenerator.ExtractAlias(shortUrl);

            return !string.IsNullOrEmpty(alias);
        }

        public bool DeleteShortUrl(string shortUrl)
        {
            if (!ValidateShortUrl(shortUrl, out string alias)) 
            {
                return false;
            }

            return _urlRepository.DeleteUrlMapping(alias);
        }

        public int GetClickCount(string shortUrl)
        {
            if (!ValidateShortUrl(shortUrl, out string alias))
            {
                return 0;
            }

            var urlMapping = _urlRepository.GetUrlMapping(alias);
         
            return urlMapping?.ClickCount ?? 0;
        }

        public string GetLongUrl(string shortUrl)
        {
            if(!ValidateShortUrl(shortUrl, out string alias))
            {
                return string.Empty;
            }

            var urlMapping = _urlRepository.GetUrlMapping(alias);
            if (urlMapping != null)
            {
                lock (_lockObject)
                {
                    urlMapping.ClickCount++;
                    _urlRepository.SaveUrlMapping(urlMapping);
                }              

                return urlMapping.LongUrl;
            }

            return string.Empty;
        }
    }
}
