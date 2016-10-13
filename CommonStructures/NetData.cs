using System;
using System.Text;

namespace Isometric.CommonStructures
{
    public struct NetData
    {
        public Version Version;
        public Encoding Encoding;

        public NetData(Version version, Encoding encoding)
        {
            Version = version;
            Encoding = encoding;
        }
    }
}

