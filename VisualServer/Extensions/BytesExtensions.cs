using System;
using System.Text;

namespace VisualServer.Extensions
{
    public static class BytesExtensions
    {
        public static string ToASCII(this byte[] bytes)
        {
            return Encoding.ASCII.GetString(bytes);
        }
    }
}

