using TinyURL.Core.Interfaces;
using TinyURL.Core.Models;
using System.Collections.Concurrent;

namespace TinyURL.Infrastructure.Repositories
{
    public class InMemoryUrlRepository : IUrlRepository
    {
        private readonly ConcurrentDictionary<string, UrlMapping> _urlMappings = new ConcurrentDictionary<string, UrlMapping>();

        public bool DeleteUrlMapping(string alias)
        {
            return _urlMappings.TryRemove(alias, out _);
        }

        public UrlMapping GetUrlMapping(string alias)
        {
             _urlMappings.TryGetValue(alias, out var urlMapping);
            return urlMapping;
        }

        public void SaveUrlMapping(UrlMapping urlMapping)
        {
            _urlMappings[urlMapping.Alias] = urlMapping;
        }
    }
}
