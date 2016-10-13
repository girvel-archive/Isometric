using System.Net.Sockets;
using System.Text;
using SocketExtensions;
using _Connection = Isometric.Server.Connection;

namespace Isometric.Server.Modules.CommandModule.Connection
{
    public class NetArgs : ISocketContainer
    {
        public Socket Socket { get; }
        public _Connection Connection { get; }

        public Encoding Encoding => Connection.Encoding;

        public NetArgs(Socket currentSocket, _Connection connection)
        {
            Socket = currentSocket;
            Connection = connection;
        }
    }
}

