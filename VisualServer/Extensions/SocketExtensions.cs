using System;
using System.Net.Sockets;
using System.Text;

namespace VisualServer.Extensions
{
    public static class SocketExtensions
    {
        public static string ReceiveAll(this Socket socket, Encoding encoding)
        {
            var currentStringBuilder = new StringBuilder();
            var receivedData = new byte[4096];

            do
            {
                var bytes = socket.Receive(receivedData);

                currentStringBuilder.Append(encoding.GetString(
                    receivedData, 0, bytes));
            }
            while (socket.Available > 0);

            return currentStringBuilder.ToString();
        }
    }
}

