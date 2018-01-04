using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;

namespace DotNetSMPP
{
    public class EventArgsPacket : EventArgs
    {
        public byte[] bytes { get; set; }
    }



    /// <summary>
    /// Class Implement Socket for communication
    /// </summary>
    class SystemSocket
    {
        /// <summary>
        /// IP End point to connect
        /// </summary>
        private IPEndPoint iep;

        /// <summary>
        /// socket , for smpp we need TCP stream socket to connect to server
        /// </summary>
        private Socket _s;



        /// <summary>
        /// Socket data Buffer container
        /// </summary>
        private byte[] _mbuff = new byte[32 * 1024];

        //private Queue<byte[]> queue = new Queue<byte[]>();


        #region Event    
        /// <summary>
        /// packet recieved Event , 
        /// packet will be added in queue
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void PacketRecieved(object sender, EventArgs e);
        public event PacketRecieved packetRecieved;
        protected virtual void OnPacketRecieved(EventArgs e) => packetRecieved?.Invoke(this, e);
        #endregion


        /// <summary>
        /// Constructor class 
        /// </summary>
        /// <param name="hostname">HOSTNAME,IPV4,IPV6</param>
        /// <param name="port"></param>
        public SystemSocket(string hostname, int port)
        {
            IPAddress ipAddr;
            var result = IPAddress.TryParse(hostname, out ipAddr);
            if (!result) throw new Exception("Invalid hostname");
            iep = new IPEndPoint(ipAddr, port);
            _s = new Socket(iep.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        }

        public bool Connect()
        {
            try
            {
                _s.Connect(iep);
                if (!_s.Connected) throw new Exception("Failed to connect");

                //here we need to create a listner to capture server returns 
                AsyncCallback asyncCallback = new AsyncCallback(OnDataRecieve);
                _s.BeginReceive(_mbuff, 0, _mbuff.Length, SocketFlags.None, asyncCallback, _s);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                //Here handle exception
                return false;
            }


        }
        public bool IsConnected
        {
            get
            {
                try { return _s.Connected; }
                catch { return false; }
            }
        }
        public void Disconnect()
        {
            try { if (this.IsConnected) { _s.Disconnect(false); } }
            catch { }
        }


        private void OnDataRecieve(IAsyncResult ar)
        {
            
            //retrive socket
            Socket sock = (Socket)ar.AsyncState;
            //retrive data
            var nBytesRecieved = sock.EndReceive(ar);
            if (nBytesRecieved > 0)
            {
                Console.WriteLine("Recieved");
                //here collect packet 
                byte[] m = new byte[nBytesRecieved];
                Array.Copy(_mbuff, m, nBytesRecieved);

                //put this packet in queue
                //queue.Enqueue(m);

                //here raise and event 
                var p = new EventArgsPacket { bytes = m };
                OnPacketRecieved(p);

                //here create some delay just to save CPU
                //
                //Thread.Sleep(5);

                //and again set buffer to recieve
                AsyncCallback asyncCallback = new AsyncCallback(OnDataRecieve);
                _s.BeginReceive(_mbuff, 0, _mbuff.Length, SocketFlags.None, asyncCallback, sock);
            }
            else
            {
                //here this socket is dead , we need to shutdown it as it is not going to recieve any thing;
                sock.Shutdown(SocketShutdown.Both);
                sock.Close();
            }



        }

        //here write to socket
        public int Write(byte[] bytestoSend)
        {
            try
            {
                if (!this.IsConnected) throw new Exception("Server Disconnected");
                return _s.Send(bytestoSend, SocketFlags.None);
            }
            catch (Exception ex)
            {
                //here handle this exception
                return 0;
            }
        }

        //internal byte[] GetPacket()
        //{
        //    if (this.queue.Count == 0) return new byte[] { };
        //    return this.queue.Dequeue();
        //}

    }
}
