using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinyURL.Core.Interfaces
{
    public interface IUrlGenerator
    {
        string CreateAlias(string origUrl);

        string ExtractAlias(string shortUrl);
    }
}
