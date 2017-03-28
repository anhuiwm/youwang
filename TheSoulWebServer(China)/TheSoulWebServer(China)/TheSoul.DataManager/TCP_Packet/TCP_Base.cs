using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;

namespace TheSoul.DataManager.TCP_Packet
{
    public class Session
    {
        private class SerializedPacket
        {
            public Packet packet;
            public byte[] bytes;
        }

        private Socket mTcpSocket;

        public bool TcpConnected { get { return mTcpSocket != null && mTcpSocket.Connected; } }

        private void TcpCleanupSocket()
        {
            if (mTcpSocket == null) return;
            mTcpSocket.Close();
            mTcpSocket.Dispose();
            mTcpSocket = null;
        }

        private static IPAddress SelectSuitableIPAddress(IPAddress[] addrs)
        {
            IPAddress result = null;

            // IPv4 네트워크를 선택합니다.
            if (result == null)
            {
                for (int i = 0; i < addrs.Length; i++)
                {
                    if (addrs[i].AddressFamily == AddressFamily.InterNetwork)
                    {
                        result = addrs[i];
                        break;
                    }
                }
            }

            return result;
        }

        public void TcpConnect(string ip, int port, int timeoutMilliseconds = 5000)
        {
            TcpCleanupSocket();
            IPAddress ipaddr;
            try
            {
                ipaddr = SelectSuitableIPAddress(Dns.GetHostAddresses(ip));
                mTcpSocket = new Socket(ipaddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
                return;
            }

            try
            {
                mTcpSocket.NoDelay = true;
                IAsyncResult result = mTcpSocket.BeginConnect(ipaddr, port, null, null);

                bool success = result.AsyncWaitHandle.WaitOne(timeoutMilliseconds, true);

                if (!success)
                {
                    // NOTE, MUST CLOSE THE SOCKET
                    mTcpSocket.Close();
                    throw new ApplicationException("Failed to connect server.");
                }
                //mTcpSocket.Connect(ipaddr, port);
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
                return;
            }
        }

        private void TcpSerializedSend(SerializedPacket p)
        {
            try
            {
                int written = mTcpSocket.Send(p.bytes, SocketFlags.None);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }

        public void TcpSend(Packet packet, bool skipEncrypt = false)
        {
            if (!TcpConnected) return;
            if (skipEncrypt == false) { packet.Encrypt(); }

            // 메인 스레드에서 미리 바이트로 바꿔 넣어둡니다.
            // 네트워크 스레드에서 하면 이것 저것 문제가 발생할듯 싶음
            SerializedPacket p = new SerializedPacket()
            {
                packet = packet,
                bytes = packet.ToBytes(),
            };

            TcpSerializedSend(p);
        }

        public void TcpClose()
        {
            TcpCleanupSocket();
        }
    }

    public class Protocol
    {
        public const System.UInt16 AF_NETWORKPACKET_IDENTITY = 0x1234;
        public const System.UInt16 AF_UDP_PACKET_IDENTITY = 0x6699;
        public const System.UInt32 AF_BUFFER_LENGTH = 4096;

        public enum ID
        {
            CS_GM_OPERATION = 8500,
            SC_GM_OPERATION = 8501    //필요할 경우 전송함. 현재 사용안함.
        }
    }

    // MUST struct for Marshal.StructToPtr Method
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Header
    {
        public const int SIZE = 16;

        public Header(Protocol.ID pid)
        {
            PID = (UInt32)pid;
            PacketNum = 0;
            BodySize = 0;
            Identity = Protocol.AF_NETWORKPACKET_IDENTITY;
            CheckSum = 0;
        }

        public Header(Byte[] buffer, int startIndex = 0)
        {
            PID = BitConverter.ToUInt32(buffer, startIndex);
            PacketNum = BitConverter.ToUInt32(buffer, startIndex + 4);
            BodySize = BitConverter.ToInt32(buffer, startIndex + 8);
            Identity = BitConverter.ToUInt16(buffer, startIndex + 12);
            CheckSum = BitConverter.ToUInt16(buffer, startIndex + 14);
        }

        public UInt32 PID;
        public UInt32 PacketNum;
        public int BodySize;
        public UInt16 Identity;
        public UInt16 CheckSum;

        public byte[] GetBytes()
        {
            return TCP_Base.StructToBytes(this);
        }

        public int GetPacketSize()
        {
            return (SIZE + BodySize);
        }
    }

    public interface IPacketizer
    {

        byte[] ToPacketData();
    }

    public class Packet
    {
        private const byte AF_CRYPT_KEY = 0x12;

        private static readonly byte[] EmptyBytes = new byte[0];

        public enum Tag
        {
            GeneratedLocal,
            TcpSnd,
            TcpRcv,
            UdpSnd,
            UdpRcv,
        }

        private Packet(Header header, byte[] rawBody)
        {
            this.header = header;
            SetBodyBytes(rawBody);
        }

        public Header header;
        private object bodyData;
        private byte[] rawBody;

        private Tag tag;

        public byte[] ToBytes()
        {
            byte[] headerBytes = header.GetBytes();
            byte[] bodyBytes = rawBody;

            byte[] data = new byte[headerBytes.Length + bodyBytes.Length];
            Buffer.BlockCopy(headerBytes, 0, data, 0, headerBytes.Length);
            Buffer.BlockCopy(bodyBytes, 0, data, headerBytes.Length, bodyBytes.Length);

            return data;
        }

        // Encrypt, Decrypt by single key
        public void Decrypt()
        {
            for (int n = 0; n < rawBody.Length; ++n)
                rawBody[n] = (byte)(rawBody[n] ^ AF_CRYPT_KEY);
        }

        public void Encrypt()
        {
            for (int n = 0; n < rawBody.Length; ++n)
                rawBody[n] = (byte)(rawBody[n] ^ AF_CRYPT_KEY);
        }

        public void SetTag(Tag tag) { this.tag = tag; }

        private void SetBodyBytes(byte[] bytes)
        {
            rawBody = bytes;
            header.BodySize = (bytes != null ? bytes.Length : 0);
        }

        private void SetBody(object boxedData)
        {
            bodyData = boxedData;
            SetBodyBytes(BoxedToBytes(boxedData));
        }

        private static byte[] BoxedToBytes(object boxed)
        {
            IPacketizer packetizer = boxed as IPacketizer;
            if (packetizer != null)
                return packetizer.ToPacketData();
            else
                return TCP_Base.StructToBytes(boxed);
        }

        public static Packet FromProtocol(Protocol.ID PID, object boxedBodyData, Tag tag = Tag.GeneratedLocal)
        {
            Packet p = new Packet(new Header(PID), null);
            p.SetBody(boxedBodyData);
            p.SetTag(tag);
            return p;
        }

        public byte[] RawBytes { get { return rawBody; } }
    }

    public class TCP_Base
    {
        public static Byte[] StructToBytes(object data)
        {
            Byte[] rawData = new Byte[Marshal.SizeOf(data)];
            GCHandle handle = GCHandle.Alloc(rawData, GCHandleType.Pinned);
            try
            {
                IntPtr rawDataPtr = handle.AddrOfPinnedObject();
                Marshal.StructureToPtr(data, rawDataPtr, false);
            }
            catch
            {
                Type dataType = data.GetType();
            }
            finally
            {
                handle.Free();
            }

            return rawData;
        }

        public static T BytesToStruct<T>(Byte[] buffer) where T : struct
        {
            T str = default(T);
            GCHandle handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);

            try
            {
                IntPtr ptr = handle.AddrOfPinnedObject();
                str = (T)Marshal.PtrToStructure(ptr, typeof(T));
            }
            catch
            {
                Type dataType = typeof(T);
                Console.Write("BytesToStruct<" + dataType.Name + ">() - failed");
            }
            finally
            {
                handle.Free();
            }

            return str;
        }

        public static T PtrToStruct<T>(IntPtr ptr) where T : struct
        {
            T str = default(T);

            try
            {
                str = (T)Marshal.PtrToStructure(ptr, typeof(T));
            }
            catch
            {
                Type dataType = typeof(T);
                Console.Write("PtrToStruct<" + dataType.Name + ">() - failed");
            }

            return str;
        }
    }
}
