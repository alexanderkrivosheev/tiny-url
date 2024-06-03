using TinyURL.Commands;
using TinyURL.Infrastructure.Extentions;

namespace TinyURL.Services
{
    internal class TinyUrlServices
    {
        private AppContext _context;
        private ICommandRegistry _commandRegistry;

        public TinyUrlServices(
            AppContext context,
            ICommandRegistry commandRegistry)
        {
            _context = context;
            _commandRegistry = commandRegistry;
        }

        private async Task<bool> HanldeInputAsync(string args)
        {
            var command = args.ExtractCommand();

            if (string.IsNullOrEmpty(command.Command))
            {
               Console.WriteLine("Command not specifed");
               return false;
            }

            var targetCommand = _commandRegistry.FindCommand(command.Command);

            if (targetCommand == null)
            {
                Console.WriteLine($"Command \"{command.Command}\" not found");
                return false;
            }

            return await targetCommand.RunAsyn(command.Parameters);
        }

        internal async Task ExecuteAsync(string args)
        {
            while (!_context.IsStoped)
            {
                try
                {
                    if (string.IsNullOrEmpty(args))
                    {
                        args = Console.ReadLine();
                    }

                    var result = await HanldeInputAsync(args);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An unexpected error has occurred.");
                }
                finally
                {
                    args = null;
                }

            }
        }        
    }
}
