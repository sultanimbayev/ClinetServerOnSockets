using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientServerOnSockets
{
    internal class HttpRequest
    {
        public string? Method;
        public string? Path;
        public string? HttpVersion;
        public IDictionary<string, string[]>? Headers { get; set; }
    }
}
