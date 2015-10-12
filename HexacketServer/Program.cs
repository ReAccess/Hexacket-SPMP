using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
//using ReAccess;

namespace HexacketServer
{
    class Program
    {
        static TcpListener TCP = new TcpListener(8484);
        static List<HexacketClient> Users = new List<HexacketClient>();
        private static string RemoteServer = "127.0.0.1";
        private static int RemotePort = 1080;

        public static byte[] BytesFill(NetworkStream Stream)
        {
            List<byte> Bytes = new List<byte>();
            while (true)
            {
                int b = Stream.ReadByte();
                if (b == -1) break;
                Bytes.Add((byte)b);
            }
            return Bytes.ToArray();
        }

        public static void StreamFill(byte[] Bytes, NetworkStream Stream)
        {
            Stream.Write(Bytes, 0, Bytes.Length);
        }

        [MTAThread]
        static void HandleUserData(object User)
        {
            HexacketClient Client = (HexacketClient)User;
            int xcode = Client.Connection.Client.Handle.ToInt32();
            byte[] bytes;
            byte[] backs;
            try
            {
                NetworkStream river = Client.Connection.GetStream();
                NetworkStream stage = Client.OutCon.GetStream();

                if (Client.Connection.Connected)
                {
                    Console.WriteLine("Transmission Active, User #" + xcode);
                }
                Users.Add(Client);
                while (Client.Connection.Connected)
                {
                    bytes = BytesFill(river);
                    StreamFill(bytes, stage);
                    backs = BytesFill(stage);
                    StreamFill(backs, river);
                    bytes.Initialize();
                    backs.Initialize();
                }
            }
            catch { }
            finally
            {
                Console.WriteLine("Transmission Deactivated, User #" + xcode);
                try { Client.Connection.Close(); }
                catch (Exception) { }
                Users.Remove(new HexacketClient() { Nickname = Client.Nickname });
                Client.Processor.Abort();
            }
        }

        [MTAThread]
        static void AcceptConnections(IAsyncResult Result)
        {
            try
            {
                HexacketClient User = new HexacketClient();
                User.Connection = TCP.EndAcceptTcpClient(Result);
                User.OutCon = new TcpClient();
                User.OutCon.Connect(RemoteServer, RemotePort);
                User.Processor = new Thread(new ParameterizedThreadStart(HandleUserData));
                // Users.Add(User);
                User.Processor.Start(User);
                Console.WriteLine("New User Connected on Socket #" + User.Connection.Client.Handle.ToInt32());
                TCP.BeginAcceptTcpClient(AcceptConnections, null);
            }
            catch
            {

            }
        }

        [STAThread]
        static void Main(string[] args)
        {
            TCP.Start();
            TCP.BeginAcceptTcpClient(AcceptConnections, null);
            Console.WriteLine("Listening to Tcp Port 8484 on All IPs.");
            string[] PrKeys = File.ReadAllLines("Server.cnf");
            foreach (string Lane in PrKeys)
            {
                if (string.IsNullOrEmpty(Lane)) continue;
                string[] Inf;
                Inf = Lane.Split(new char[1] { ' ' }, 2);
                if (Inf[0].ToLower() == "hostname") RemoteServer = Inf[1];
                else if (Inf[0].ToLower() == "hostport") RemotePort = int.Parse(Inf[1]);
            }
            new ManualResetEvent(false).WaitOne();
        }
    }
}
