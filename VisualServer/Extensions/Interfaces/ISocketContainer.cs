using System;
using System.Net.Sockets;

namespace VisualServer.Extensions.Interfaces
{
    public interface ISocketContainer
    {
        Socket Socket { get; }

        Server Server { get; }
    }
}

