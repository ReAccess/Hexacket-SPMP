using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace HexacketServer
{
    class HexacketClient
    {
        public string Nickname = "";
        public Thread Processor = null;
        public TcpClient Connection = null;
        public TcpClient OutCon = null;
    }
}
