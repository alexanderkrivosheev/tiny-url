using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinyURL.Infrastructure.Extentions
{
    public static class ArgsExtentions
    {
        public static (string Command, Dictionary<string,string> Parameters) ExtractCommand(this string args) 
        {
            if (string.IsNullOrEmpty(args))
            {
                return (string.Empty, new Dictionary<string, string>());
            }

            args = args.Trim();

            var inputs = args.Split(' ');

            var commandName = inputs.First();

            if (string.IsNullOrEmpty(commandName))
            {
                return (string.Empty, new Dictionary<string, string>());
            }

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string currentKey = null;
            foreach (var input in inputs.Skip(1))
            {
                if (string.IsNullOrEmpty(input))
                {
                    continue;
                }

                if (input.StartsWith("-"))
                {
                    var key = input.TrimStart('-');

                    if (string.IsNullOrEmpty(key))
                    {
                        continue;
                    }

                    if (!parameters.ContainsKey(key))
                    {
                        parameters.Add(key, string.Empty);
                    }

                    currentKey = key;
                }
                else
                {
                    if (string.IsNullOrEmpty(currentKey))
                    {
                        continue;
                    }
                    else
                    {
                        var value = input;

                        if (!string.IsNullOrEmpty(value))
                        {
                            value = value.Trim('\'');
                            value = value.Trim();
                        }

                        parameters[currentKey] = value;
                        currentKey = null;
                    }
                }
            }

            return (commandName, parameters);

        }
    }
}
