using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientServerOnSockets
{
    internal class RequestHandler
    {

        public async Task SendResponse(Stream responseStream, HttpResponse response)
        {
            var writer = new StreamWriter(responseStream);
            await writer.WriteLineAsync(response.StatusLine);
            if(response.Headers != null)
            {
                foreach(var header in response.Headers)
                {
                    var headerLine = $"{header.Key}: ";
                    headerLine += String.Join(",", header.Value);
                    await writer.WriteLineAsync(headerLine);
                }
            }
            writer.WriteLine();

            if(response.Body != null && response.Body.CanRead)
            {
                await response.Body.CopyToAsync(responseStream);
            }
            await writer.WriteLineAsync();
        }

        public async Task<HttpRequest> HttpRequestFrom(Stream stream)
        {
            var requestReader = new StreamReader(stream);
            // GET /index HTTP/1.1
            var statusLine = await requestReader.ReadLineAsync();
            
            if(statusLine == null) { return null; }
            var statusLineData = statusLine.Split(' ');

            var httpReq = new HttpRequest();

            httpReq.Method = statusLineData[0];
            httpReq.Path = statusLineData[1];
            httpReq.HttpVersion = statusLineData[2];
            httpReq.Headers = new Dictionary<string, string[]>();

            string? headerLine;
            do
            {
                //Accept: text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9
                headerLine = await requestReader.ReadLineAsync();

                if (string.IsNullOrEmpty(headerLine)) { break; }

                var headerKey = headerLine.Split(':')[0];
                var headerValues = headerLine.Split(':')[1].Split(",").Select(v => v.Trim()).ToArray();
                httpReq.Headers.Add(headerKey, headerValues);


            } while (!string.IsNullOrEmpty(headerLine));

            return httpReq;

        }
    }
}
