using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ClientServerOnSockets
{
    internal class Server : IDisposable
    {
        const string ROOT_FOLDER = "C:\\www";
        const string SERVER_IP = "127.0.0.1";
        // Establish the local endpoint for the socket.  
        // Dns.GetHostName returns the name of the
        // host running the application.  
        //IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
        private Socket Listener;
        private bool _started = false;

        public void Dispose()
        {
            Listener.Dispose();
        }

        public async void Start(int port, int backlog = 10)
        {

            IPAddress ipAddress = IPAddress.Parse(SERVER_IP);
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, port);
            Listener = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            Listener.Bind(localEndPoint);
            Listener.Listen(backlog);
            _started = true;
             
            while (_started)
            {
                var socket = Listener.Accept();
                if (socket != null)
                {
                    _ = await HandleRequest(socket);
                }
            }
        }

        public async Task<RequestHandler> HandleRequest(Socket socket)
        {
            var requestHandler = new RequestHandler();
            await requestHandler.HandleRequest(new NetworkStream(socket));
            return requestHandler;
        }

        public void Stop()
        {
            _started = false;
            if (Listener == null)
            {
                return;
            }
            Listener.Close();
        }


    }
}
