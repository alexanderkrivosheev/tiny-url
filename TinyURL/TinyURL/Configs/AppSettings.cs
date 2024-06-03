using TinyURL.Core.Interfaces;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinyURL.Configs
{
    internal class AppSettings : IUrlShorteningSettings
    {
        private IOptions<UrlOptions> _urlOptions { get; }

        public AppSettings(IOptions<UrlOptions> urlOptions)
        {
            _urlOptions = urlOptions;
        }

        public string Host => _urlOptions.Value.Host;

        public string Schema => _urlOptions.Value.Schema;

        
    }
}
