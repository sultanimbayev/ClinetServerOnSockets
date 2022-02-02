// See https://aka.ms/new-console-template for more information
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using ClientServerOnSockets.Attributes;

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
        Console.WriteLine("Text received :");
        Console.WriteLine(data);

        var path = data.Split("r\n")[0].Split(" ")[1];

        object myVar = path;



        var statusLine = "HTTP/1.1 200 OK\r\n";
        var headers = @"Content-Type: text/html
Authorization: Bearer afds54a56sd4f6a5s4df654asf
My-Custom-Header: Hello World!
";

        var controllers = Assembly.GetExecutingAssembly().DefinedTypes
            .Where(t => t.CustomAttributes.Any((CustomAttributeData attr) => attr.AttributeType == typeof(ControllerAttribute)));

        Type controllerType = null;
        MethodInfo controllerMethod = null;

        foreach (var c in controllers)
        {
            foreach (var m in c.GetMethods()) 
            {
                var routeAttr = m.GetCustomAttribute<RouteAttribute>();
                if (routeAttr != null && routeAttr.Path == path)
                {
                    controllerType = c;
                    controllerMethod = m;
                    break;
                }

            } 

        }

        byte[] msg = Encoding.ASCII.GetBytes("");
        if (controllerType == null || controllerMethod == null)
        {
            statusLine = "HTTP/1.1 404 Not Found\r\n";
        }
        else
        {
            //var controller = new MyController();
            var controller = Activator.CreateInstance(controllerType);

            byte[] result = (byte[])(controllerMethod.Invoke(controller, new object[] { }));

            msg = result;
        }

        //byte[] msg = Encoding.ASCII.GetBytes(data);

        handler.Send(Encoding.ASCII.GetBytes(statusLine + headers + "\r\n"));
        handler.Send(msg);
        //handler.SendFile("61b2e000e0ae4.jpg");
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
