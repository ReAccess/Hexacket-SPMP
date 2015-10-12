using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Net.Security;
using System.Windows.Forms;
using System.IO;
//using ReAccess.Net;

namespace Hexacket
{
    static class HexacketUtility
    {
        public static string BuildHttpRequest(string Method, string Hostname, string Path)
        {
            string RequestBuild = Method + " " + Path + " HTTP/1.1";
            RequestBuild += "\r\n";
            RequestBuild += "Host: " + Hostname;
            RequestBuild += "\r\n";
            RequestBuild += "Content-Length: 0";
            RequestBuild += "\r\n";
            RequestBuild += "\r\n";
            return RequestBuild;
        }
    }

    static class Program
    {
        static Socket PSocket = null;

        static List<Socket> ClientSocket = new List<Socket>();
        static List<Thread> ClientThread = new List<Thread>();

        static ManualResetEvent ThreadHandler = new ManualResetEvent(false);
        static ManualResetEvent StreamSyncHandler = new ManualResetEvent(false);

        static string RemoteServer = "";
        static int RemotePort = 8484;

        static readonly string[] TCPProtId = { "http", "https", "ftp", "tcp" };
        private static FormMain MegaForm; // DI

        [MTAThread]
        static void ListenQuery(object Handle)
        {
            int CH = (int)Handle;
            Socket ConSock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                ConSock.Connect(RemoteServer, RemotePort);
                List<byte> ConBuffer = new List<byte>();
                NetworkStream NS = new NetworkStream(ClientSocket[CH]);
                while (true)
                {
                    int b = NS.ReadByte();
                    if (b == -1) break;
                    ConBuffer.Add((byte)b);
                }
                NS.Flush();

            }
            catch (Exception ex)
            {
                Console.WriteLine("CON " + CH + " > " + ex.Message);
                goto the_end;
            }

            int IndexOfSent = 0;
            int IndexOfRecv = 0;

            while (ClientSocket[CH].Connected)
            {
                try
                {
                    List<byte> XBuffer = new List<byte>();
                    Connection Formx = new Connection(); // DI
                    Formx.Text = $"CONNECTION #{CH} STATUS"; // DI
                    // MegaForm.Invoke((MethodInvoker)delegate () { Formx.Show(); }); // DI
                    while (ClientSocket[CH].Available == 0) Thread.Sleep(1);
                    MegaForm.Invoke((MethodInvoker)delegate () { IndexOfSent = MegaForm.LB.Items.Add($"C{CH}: Data Available: {ClientSocket[CH].Available}"); }); // DI
                    while (true)
                    {
                        NetworkStream NS = new NetworkStream(ClientSocket[CH]);
                        int b = NS.ReadByte();
                        if (b == -1) break;
                        XBuffer.Add((byte)b);
                    }
                    Console.WriteLine("CON " + CH + " > " + "DATA UPSTREAM GATHERED");
                    // MegaForm.Invoke((MethodInvoker)delegate () { IndexOfSent = MegaForm.LB.Items.Add($"C{CH}: Data Sent: {XBuffer.Count}"); }); // DI
                    
                    ConSock.Send(XBuffer.ToArray());
                    if (ConSock.Connected)
                    {
                        Console.WriteLine("CON " + CH + " > " + "STATUS 2 STARTED");
                        List<byte> RBuffer = new List<byte>();
                        NetworkStream OutStream = new NetworkStream(ConSock);
                        NetworkStream InStream = new NetworkStream(ClientSocket[CH]);
                        Console.WriteLine("CON " + CH + " > " + "NETSTREAM THREADED");
                        while (true)
                        {
                            int b = OutStream.ReadByte();
                            if (b == -1) break;
                            RBuffer.Add((byte)b);
                            InStream.WriteByte((byte)b);
                        }
                        InStream.Flush();
                        Console.WriteLine("CON " + CH + " > " + "BUFFER WRITTEN!");
                        MegaForm.Invoke((MethodInvoker)delegate () { IndexOfRecv = MegaForm.LR.Items.Add($"C{CH}: Data Recv: {RBuffer.Count}"); }); // DI
                    }
                    else break;
                    // NS.Flush();
                }
                catch (Exception ex)
                {
                    string rq = "HTTP/1.0 464 Median Error\r\nContent-Length: " + ex.Message.Length + "\r\n\r\n" + ex.Message;
                    Console.WriteLine("CON " + CH + " > " + ex.Message);
                        
                    ClientSocket[CH].Send(Encoding.ASCII.GetBytes(rq));
                    break;
                }
                Thread.Sleep(10);
            }
        the_end:
            ConSock.Close();
            ClientSocket[CH].Close();
            ClientSocket[CH] = null;
            ClientThread[CH].Abort();
            ClientThread[CH] = null;
            Console.WriteLine("CON " + CH + " > " + "TRANSMISSION TERMINATED");
        }

        [STAThread]
        static void ListenApplicator(IAsyncResult Result)
        {
            Socket Current; int CH;
            List<byte> XBuffer = new List<byte>();
            IAsyncResult Result2 = Result;
            Current = PSocket.EndAccept(Result);
            if ((CH = ClientSocket.IndexOf(null)) == -1)
            {
                CH = ClientSocket.Count;
                ClientSocket.Add(Current);
                ClientThread.Add(new Thread(new ParameterizedThreadStart(ListenQuery)));
            }
            else
            {
                ClientSocket[CH] = Current;
                ClientThread[CH] = new Thread(new ParameterizedThreadStart(ListenQuery));
            }
            
            ClientThread[CH].Start(CH);
            PSocket.BeginAccept(new AsyncCallback(ListenApplicator), null);
        }

        [MTAThread]
        static void Main(string[] args)
        {
            PSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            PSocket.Bind(new IPEndPoint(new IPAddress(new byte[4] { 127, 0, 0, 1 }), 8585));
            PSocket.Listen(1024);
            MegaForm = new FormMain();
            PSocket.BeginAccept(new AsyncCallback(ListenApplicator), null);
            string[] PrKeys = File.ReadAllLines("Proxy.cnf");
            foreach (string Lane in PrKeys)
            {
                if (string.IsNullOrEmpty(Lane)) continue;
                string[] Inf;
                Inf = Lane.Split(new char[1] { ' ' }, 2);
                if (Inf[0].ToLower() == "hostname") RemoteServer = Inf[1];
                else if (Inf[0].ToLower() == "hostport") RemotePort = int.Parse(Inf[1]);
            }
            Console.WriteLine("Listening... PORT 8585 LOCALHOST");
            Application.Run(MegaForm);
        }
    }
}
