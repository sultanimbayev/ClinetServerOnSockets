using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientServerOnSockets.Attributes
{
    public class RouteAttribute : Attribute
    {
        public RouteAttribute(string path)
        {
            Path = path;
        }

        public string Path { get; private set; }
    }
}
