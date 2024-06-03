using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinyURL.Commands
{
    public class TinyURLCommandRegistry : ICommandRegistry
    {
        private IEnumerable<ICommand> _commands;

        public TinyURLCommandRegistry(IEnumerable<ICommand> commands) 
        {
            _commands = commands;
        }

        public ICommand FindCommand(string name)
        {
            return _commands.FirstOrDefault(c => string.Equals(c.Name, name, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}
