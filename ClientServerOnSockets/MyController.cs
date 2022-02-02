using ClientServerOnSockets.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientServerOnSockets
{
    [Controller]
    public class MyController
    {
        [Route("/")]
        public byte[] Home()
        {

            var content =
                @"<html>
                   <body>
                     <h1> Hello, World!</h1>
                   </body>
                </html>" + "\r\n\r\n";

            return Encoding.ASCII.GetBytes(content);

        }
    }
}
