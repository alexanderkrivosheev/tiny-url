using TinyURL.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinyURL.Core.Interfaces
{
    public interface IUrlRepository
    {
        UrlMapping GetUrlMapping(string alias);
        void SaveUrlMapping(UrlMapping urlMapping);
        bool DeleteUrlMapping(string alias);
    }
}
