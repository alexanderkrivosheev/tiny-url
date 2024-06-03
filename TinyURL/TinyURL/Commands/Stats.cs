using TinyURL.Core.Interfaces;

namespace TinyURL.Commands
{
    public class Stats : ICommand
    {
        private IUrlShorteningService _urlShorteningService;
        public Stats(IUrlShorteningService urlShorteningService)
        {
            _urlShorteningService = urlShorteningService;
        }

        public string Name => "stats";

        public string Description =>
@"Used to retrieve click statistics for a short URL
    -url: the short URL to get statistics for (required)
    stats -url 'http://tinyurl.com/example'
";

        public async Task<bool> RunAsyn(Dictionary<string, string> args)
        {
            if(!args.ContainsKey("url"))
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

            var count = _urlShorteningService.GetClickCount(shortUrl);

            Console.WriteLine($"URL {shortUrl} has been requested {count} times");

            return true;    
        }
    }
}
