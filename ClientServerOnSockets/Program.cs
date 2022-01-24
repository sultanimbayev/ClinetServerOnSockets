// See https://aka.ms/new-console-template for more information
using System.Net;
using System.Net.Sockets;
using System.Text;

string data = null;
// Data buffer for incoming data.  
byte[] bytes = new Byte[1024];

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
            if (data.EndsWith("\r\n\r\n"))
            {
                break;
            }
        }

        // Show the data on the console.  
        Console.WriteLine("Text received : {0}", data);

        var header = "HTTP/1.1 200 OK\r\n\r\n";
        var content = 
            @"<html>
               <body>
                 <h1> Hello, World!</h1>
               </body>
            </html>" + "\r\n\r\n";

        // Echo the data back to the client.  
        byte[] msg = Encoding.ASCII.GetBytes(data);

        handler.Send(Encoding.ASCII.GetBytes(header + content));
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
