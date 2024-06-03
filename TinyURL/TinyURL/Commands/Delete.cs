using TinyURL.Core.Interfaces;

namespace TinyURL.Commands
{
    public class Delete : ICommand
    {
        private IUrlShorteningService _urlShorteningService;
        public Delete(IUrlShorteningService urlShorteningService)
        {
            _urlShorteningService = urlShorteningService;
        }

        public string Name => "delete";

        public string Description =>
@"Used to delete a short URL
    -url: the short URL to delete (required).
    delete -url 'http://tinyurl.com/example'
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

            var result = _urlShorteningService.DeleteShortUrl(shortUrl);

            if (result)
            {
                Console.WriteLine($"Short Url {shortUrl} has been deleted");
            }
            else
            {
                Console.WriteLine($"Short Url {shortUrl} not found");
            }

            return true;
        }
    }
}
