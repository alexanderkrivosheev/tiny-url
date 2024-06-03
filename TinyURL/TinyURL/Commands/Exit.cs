using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinyURL.Commands
{
    public class Exit : ICommand
    {
        private AppContext _context;
        public Exit(AppContext context)
        {
            _context = context;
        }
        public string Name => "exit";

        public string Description => 
@"Used to exit the application
";

        public async Task<bool> RunAsyn(Dictionary<string, string> args)
        {
            _context.Stop();
            return true;
        }
    }
}
