using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientServerOnSockets
{
    internal interface IResponseBuilder
    {
        Task<HttpResponse> BuildResponseAsync(HttpRequest request);
    }
}
