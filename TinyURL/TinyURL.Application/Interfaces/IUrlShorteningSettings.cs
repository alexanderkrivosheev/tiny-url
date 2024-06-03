using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinyURL.Core.Interfaces
{
    public interface IUrlShorteningSettings
    {
        public string Host { get;}

        public string Schema { get;}
    }
}
