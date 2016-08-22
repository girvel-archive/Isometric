using System;
using System.Net.Sockets;
using VisualServer.Extensions.Interfaces;
using _Server = VisualServer.Server;
using _Connection = VisualServer.Connection;

namespace VisualServer.Modules.CommandModule.Connection
{
    public class NetArgs : ISocketContainer
    {
        public Socket Socket { get; }
        public _Connection Connection { get; }

        public _Server Server => Connection.ParentServer;

        public NetArgs(Socket currentSocket, _Connection connection)
        {
            Socket = currentSocket;
            Connection = connection;
        }
    }
}

