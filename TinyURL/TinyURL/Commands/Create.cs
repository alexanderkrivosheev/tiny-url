using TinyURL.Core.Interfaces;

namespace TinyURL.Commands
{
    public class Create : ICommand
    {
        private IUrlShorteningService _urlShorteningService;
        
        public Create(IUrlShorteningService urlShorteningService)
        {
            _urlShorteningService = urlShorteningService;
        }

        public string Name => "create";

        public string Description =>
@"Used to create a short URL.
    -url: the original URL (required)
    -alias: the desired short link (optional)
    create -url 'http://example.com/long-url' -alias 'example'
";

        public async Task<bool> RunAsyn(Dictionary<string, string> args)
        {
            if(!args.ContainsKey("url"))
            {
                Console.WriteLine("-url parameter not specified");
                return false;
            }

            var origUrl = args["url"];

            if (string.IsNullOrEmpty(origUrl))
            {
                Console.WriteLine("-url parameter cannot be empty");
                return false;
            }

            if (args.TryGetValue("alias", out string alias))
            {
                if (string.IsNullOrEmpty(alias))
                {
                    Console.WriteLine("-alias parameter cannot be empty");
                    return false;
                }
            }
         
            var result = _urlShorteningService.CreateShortUrl(origUrl, alias);

            Console.WriteLine($"{result}");

            return true;    
        }
    }
}
