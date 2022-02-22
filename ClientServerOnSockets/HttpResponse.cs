using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientServerOnSockets
{
    internal class HttpResponse
    {
        public string HttpVersion { get; set; }
        public int StatusCode { get; set; }
        public string StatusMsg { get; set; }
        public string? StatusLine { get => $"{HttpVersion} {StatusCode} {StatusMsg}"; }
        public IDictionary<string, string[]>? Headers { get; set; }
        public Stream Body { get; set; }

    }
}
