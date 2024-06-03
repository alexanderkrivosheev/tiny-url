using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinyURL.Commands
{
    public interface ICommandRegistry
    {
        ICommand FindCommand(string name);
    }
}
