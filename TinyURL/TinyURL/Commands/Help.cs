using Microsoft.Extensions.DependencyInjection;

namespace TinyURL.Commands
{
    public class Help : ICommand
    {
        private IServiceProvider _provider;
        public Help(IServiceProvider provider)
        {
            _provider = provider;
        }
        public string Name => "help";

        public string Description => 
@"Used to get all commands
";

        public async Task<bool> RunAsyn(Dictionary<string, string> args)
        {
            var commands = _provider.GetRequiredService<IEnumerable<ICommand>>();

            Console.WriteLine(
@"Adroit TinyURL Help
");
            foreach (var command in commands)
            {
                Console.WriteLine($" {command.Name}");
                Console.WriteLine($" {command.Description}");               
            }
            return true;
        }
    }
}
