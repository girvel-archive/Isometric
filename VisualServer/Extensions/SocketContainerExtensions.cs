using System;
using VisualServer.Extensions.Interfaces;

namespace VisualServer.Extensions
{
    public static class SocketContainerExtensions
    {
        public static void Send(this ISocketContainer container, string message)
        {
            container.Socket.Send(container.Server.Encoding.GetBytes(message));
        }
    }
}

