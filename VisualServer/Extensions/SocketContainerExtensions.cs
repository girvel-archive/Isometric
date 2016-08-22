using System;
using VisualServer.Extensions.Interfaces;
using BinarySerializationExtensions;

namespace VisualServer.Extensions
{
    public static class SocketContainerExtensions
    {
        public static void Send(this ISocketContainer container, string message)
        {
            container.Socket.Send(container.Server.Encoding.GetBytes(message));
        }

        public static string ReceiveAll(this ISocketContainer container)
        {
            return container.Socket.ReceiveAll(container.Server.Encoding);
        }

        public static T Deserialize<T>(this ISocketContainer container, string data)
            where T : new()
        {
            return container.Server.Encoding.GetBytes(data).ByteDeserialize<T>();
        }
    }
}

