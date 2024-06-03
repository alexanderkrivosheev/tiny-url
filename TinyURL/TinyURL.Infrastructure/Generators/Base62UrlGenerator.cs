using TinyURL.Core.Interfaces;
using System.Text;

namespace TinyURL.Infrastructure.Generators
{
    public class Base62UrlGenerator : IUrlGenerator
    {
        private int _count = 0;
      
        public Base62UrlGenerator()
        {
        }

        private const string Characters = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

        public string CreateAlias(string origUrl)
        {
            var nextId = Interlocked.Increment(ref _count);

            var sb = new StringBuilder();

            do
            {
                sb.Insert(0, Characters[(int)(nextId % 62)]);
                nextId /= 62;
            } while (nextId > 0);

            return sb.ToString();
        }

        public string ExtractAlias(string shortUrl)
        {
            if (string.IsNullOrEmpty(shortUrl))
            {
                return shortUrl;
            }

            shortUrl = shortUrl.Trim('/');

            if (!Uri.IsWellFormedUriString(shortUrl, UriKind.Absolute))
            {
                return shortUrl;
            }

            int index = shortUrl.LastIndexOf('/');
            if (index == -1)
            {
                return shortUrl;
            }

            var alias = shortUrl.Substring(index, shortUrl.Length - index);

            return alias.Trim('/') ;
        }
    }
}
