// See https://aka.ms/new-console-template for more information
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

string data = null;
// Data buffer for incoming data.  
byte[] bytes = new Byte[1024];

string rootFolder = "C:\\www";

// Establish the local endpoint for the socket.  
// Dns.GetHostName returns the name of the
// host running the application.  
//IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 11000);

using var listener = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
// Bind the socket to the local endpoint and
// listen for incoming connections.  
try
{
    listener.Bind(localEndPoint);
    listener.Listen(10);

    // Start listening for connections.  
    while (true)
    {
        Console.WriteLine("Waiting for a connection...");
        // Program is suspended while waiting for an incoming connection.  
        Socket handler = listener.Accept();
        data = null;

        // An incoming connection needs to be processed.  
        while (true)
        {
            int bytesRec = handler.Receive(bytes);
            data += Encoding.ASCII.GetString(bytes, 0, bytesRec); 
            if (data.EndsWith("\r\n\r\n") || bytesRec == 0)
            {
                break;
            }
        }

        // Show the data on the console.  
        Console.WriteLine("Text received : {0}", data);


        var path = data.Split("\r\n")[0].Split(" ")[1];
        var headersValues = data.Split("\r\n").Skip(1).Where(line => line.Contains(":")).Select(line =>
        {
            var headerKey = line.Split(": ")[0];
            var headerValues = line.Split(": ")[1].Split(",");
            return new KeyValuePair<string, string[]>(headerKey, headerValues);
        });
        var headers = new Dictionary<string, string[]>(headersValues);

        var auth = "";
        if (headers.ContainsKey("Authorization"))
        {
            auth = headers["Authorization"][0];
        }

        byte[] responseData;

//        var headers = @"Authorization: Bearer afds54a56sd4f6a5s4df654asf
//My-Custom-Header: Hello World!
//";

        var statusLine = "HTTP/1.1 200 OK\r\n";

        if (path == "/")
        {
            path = "/index.html";
        }
        if(path == "/admin")
        {

        }

        var filePath = Path.Combine(rootFolder, path.Substring(1));

        if (File.Exists(filePath))
        {
            responseData = File.ReadAllBytes(filePath);
        }
        else
        {
            responseData = new byte[0];
            statusLine = "HTTP/1.1 404 Not Found\r\n";
        }


        handler.Send(Encoding.ASCII.GetBytes(statusLine /*+ headers*/ + "\r\n"));
        handler.Send(responseData);
        handler.Send(Encoding.ASCII.GetBytes("\r\n\r\n"));
        handler.Shutdown(SocketShutdown.Both);
        handler.Close();
    }

}
catch (Exception e)
{
    Console.WriteLine(e.ToString());
}

Console.WriteLine("\nPress ENTER to continue...");
Console.Read();
