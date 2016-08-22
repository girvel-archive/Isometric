using System;
using VisualServer.Extensions.Interfaces;
using System.Net.Sockets;
using _Server = VisualServer.Server;

namespace VisualServer.Modules.CommandModule.Server
{
    public class NetArgs : ISocketContainer
    {
        public Socket Socket { get; }
        public _Server Server { get; }

        public NetArgs(Socket socket, _Server server)
        {
            Socket = socket;
            Server = server;
        }
    }
}

