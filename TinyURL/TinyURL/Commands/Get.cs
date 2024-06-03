using TinyURL.Core.Interfaces;

namespace TinyURL.Commands
{
    public class Get : ICommand
    {
        private IUrlShorteningService _urlShorteningService;
        public Get(IUrlShorteningService urlShorteningService)
        {
            _urlShorteningService = urlShorteningService;
        }

        public string Name => "get";

        public string Description =>
@"Used to retrieve the original URL from a short URL
    -url: the short URL to look up (required).
    get -url 'http://tinyurl.com/example'
";

        public async Task<bool> RunAsyn(Dictionary<string, string> args)
        {
            if (!args.ContainsKey("url"))
            {
                Console.WriteLine("-url parameter not specified");
                return false;
            }

            var shortUrl = args["url"];

            if (string.IsNullOrEmpty(shortUrl))
            {
                Console.WriteLine("-url parameter cannot be empty");
                return false;
            }

            var longUrl = _urlShorteningService.GetLongUrl(shortUrl);

            if (string.IsNullOrEmpty(longUrl))
            {
                Console.WriteLine($"Short Url \'{shortUrl}\' not found");
                return false;
            }

            Console.WriteLine($"{longUrl}");

            return true;
        }
    }
}
