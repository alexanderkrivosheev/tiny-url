
namespace TinyURL.Commands
{
    public class Welcome : ICommand
    {        
        public Welcome()
        {
        }

        public string Name => "welcome";

        public string Description => 
@"Used to get main features
";

        public async Task<bool> RunAsyn(Dictionary<string, string> args)
        {
            string text =

@"Welcome to Adroit TinyURL – your reliable helper for creating short links!

Please familiarize yourself with the available commands for managing the program:

    - Generate a short URL from a long one.
    - Remove an existing short URL and its associated long URL.
    - Retrieve the original long URL from a short one.
    - Get detailed statistics on the number of times a short URL has been clicked.
    - Create a custom short URL or let the app randomly generate one.
    - Edit or delete your existing short URLs.

    Please use 'help' command to get a detailed list of available commands.

    Best Regards,
    Alexander
";

            Console.WriteLine(text);

            return await Task.FromResult(true);
        }
    }
}
