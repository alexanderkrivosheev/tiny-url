using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinyURL.Core.Models
{
    public class UrlMapping
    {
        public string Alias { get; set; }
        public string ShortUrl { get; set; }
        public string LongUrl { get; set; }
        public int ClickCount { get; set; }
    }
}
