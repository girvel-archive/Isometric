using System.Text;
using System.Net.Sockets;
using _Connection = VisualServer.Connection;
using SocketExtensions;

namespace VisualServer.Modules.CommandModule.Connection
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

