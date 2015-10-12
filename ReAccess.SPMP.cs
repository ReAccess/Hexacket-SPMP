using System;
using System.Text;
using System.Security.Cryptography;
using System.Security.Authentication;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Collections.Generic;

namespace ReAccess
{
    namespace Net
    {
        namespace SPMP
        {
            /// <summary>
            /// Identifies The Type of AUTH Request.
            /// </summary>
            public enum SpmpAuthAttempt
            {
                /// <summary>
                /// Requests an Identification From The Server and Network Information.
                /// </summary>
                Handshake,

                /// <summary>
                /// Sends The Credentials to The Server and Identifies Itself to Be Authorised.
                /// </summary>
                Authorise,

                /// <summary>
                /// Requests Communication Key and Establishes Secure Packed Mediator Connection.
                /// </summary>
                GetKey
            }

            /// <summary>
            /// Indicates The Length of Hash/Encryption Algorithms to Be Used Within Spmp Connections.
            /// </summary>
            public enum SpmpStrength:byte
            {
                x128, x256, x384, x512
            }

            public class SpmpConnection
            {
                public readonly SpmpStrength SecurityStrength;

                public readonly byte[] AuthKey = null;

                public SpmpConnection(SpmpStrength Strength)
                {
                    SecurityStrength = Strength;
                    AuthKey = new byte[((byte)Strength + 1) * 128];
                }

                private byte[] HashX(int Input)
                {
                    HashAlgorithm Hasher = null;
                    switch (SecurityStrength)
                    {
                        case SpmpStrength.x128: { Hasher = SHA1.Create(); break; }
                        case SpmpStrength.x256: { Hasher = SHA256.Create(); break; }
                        case SpmpStrength.x384: { Hasher = SHA384.Create(); break; }
                        case SpmpStrength.x512: { Hasher = SHA512.Create(); break; }
                    }
                    return Hasher.ComputeHash(BitConverter.GetBytes(Input));
                }

                private byte[] HashX(byte[] Input)
                {
                    HashAlgorithm Hasher = null;
                    switch (SecurityStrength)
                    {
                        case SpmpStrength.x128: { Hasher = SHA1.Create(); break; }
                        case SpmpStrength.x256: { Hasher = SHA256.Create(); break; }
                        case SpmpStrength.x384: { Hasher = SHA384.Create(); break; }
                        case SpmpStrength.x512: { Hasher = SHA512.Create(); break; }
                    }
                    return Hasher.ComputeHash(Input);
                }

                /// <summary>
                /// Builds a AUTH Request to Be Sent to The SPMP Server.
                /// </summary>
                /// <param name="Attempt">Type of SPMP-AUTH Attempt.</param>
                /// <param name="EndPoint">The </param>
                /// <param name="AuthKey"></param>
                /// <returns></returns>
                public byte[] BuildAuth(SpmpAuthAttempt Attempt, IPEndPoint EndPoint, string Username, string Password)
                {
                    // Request Header : (A)UTH / (D)EMAND / (N)EGOTIATE / (C)LOSE
                    // Auth Commands : (H)ANDSHAKE / (A)UTHORISE / GET(K)EYS
                    // IP Format : B1.B2.B3.B4 -> B1+B2+B3+B4

                    if (EndPoint.AddressFamily != AddressFamily.InterNetwork) throw new Exception("Unsupported SPMP Address Family.");

                    List<byte> RqStr = new List<byte>();
                    RqStr.Add((byte)'A');
                    switch (Attempt)
                    {
                        case SpmpAuthAttempt.Handshake:
                        {
                            RqStr.Add((byte)'H');
                            string[] hostname = EndPoint.Address.ToString().Split('.');
                            foreach (string ipb in hostname) RqStr.Add(byte.Parse(ipb));
                            int port = EndPoint.Port;
                            RqStr.Add((byte)(port & 0xFF));
                            RqStr.Add((byte)(port >> 8 & 0xFF));
                            RqStr.Add(0);
                            break;
                        }
                        case SpmpAuthAttempt.Authorise:
                        {
                            RqStr.Add((byte)'A');
                            byte[] username = Encoding.ASCII.GetBytes(Username);
                            byte[] password = Encoding.ASCII.GetBytes(Password);
                            RqStr.Add((byte)username.Length);
                            RqStr.AddRange(username);
                            byte[] passauth = ArrayUtility<byte>.Concate(password, AuthKey);
                            RqStr.AddRange(HashX(passauth));
                            RqStr.Add(0);
                            break;
                        }
                    }
                    RqStr.Add(0);
                    RqStr.Add(0);
                    return RqStr.ToArray();
                }

                public byte[] BuildNeg(byte[] IP, int Port, byte[] Request)
                {
                    List<byte> RqStr = new List<byte>();
                    RqStr.Add((byte)'N');
                    foreach (byte p in IP) RqStr.Add(p);
                    RqStr.Add((byte)(Port & 0xFF));
                    RqStr.Add((byte)(Port >> 8 & 0xFF));
                    RqStr.AddRange(Request);
                    RqStr.Add(0);
                    return RqStr.ToArray();
                }
            }
        }
    }
}