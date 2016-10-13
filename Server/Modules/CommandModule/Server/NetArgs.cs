using System.Net.Sockets;
using System.Text;
using SocketExtensions;
using _Server = Isometric.Server.Server;

namespace Isometric.Server.Modules.CommandModule.Server
{
    public class NetArgs : ISocketContainer
    {
        public Socket Socket { get; }
        public _Server Server { get; }

        public Encoding Encoding => Server.Encoding;

        public NetArgs(Socket socket, _Server server)
        {
            Socket = socket;
            Server = server;
        }
    }
}

