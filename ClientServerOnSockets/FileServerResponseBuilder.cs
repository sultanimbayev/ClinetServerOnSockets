using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientServerOnSockets
{
    internal class FileServerResponseBuilder : IResponseBuilder
    {
        public string RootPath { get; private set; }
        public FileServerResponseBuilder(string rootPath = "C:\\www")
        {
            RootPath = rootPath;
        }

        public async Task<HttpResponse> BuildResponseAsync(HttpRequest request)
        {
            var response = new HttpResponse();
            response.HttpVersion = "HTTP/1.1";
            response.StatusCode = 200;
            response.StatusMsg = "OK";
            var path = request.Path?.Substring(1);

            if (string.IsNullOrEmpty(path))
            {
                path = "index.html";
            }

            var filePath = Path.Combine(RootPath, path);
            if (File.Exists(path))
            {
                var fileStream = File.OpenRead(filePath);
                response.Body = fileStream;
            }
            else
            {
                response.Body = null;
                response.StatusCode = 404;
                response.StatusMsg = "Not Found";
            }
            return await Task.FromResult(response);
        }
    }
}
