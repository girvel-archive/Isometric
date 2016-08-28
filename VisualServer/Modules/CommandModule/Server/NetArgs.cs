using System.Net.Sockets;
using _Server = VisualServer.Server;
using System.Text;
using SocketExtensions;

namespace VisualServer.Modules.CommandModule.Server
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

