/*

 
 
 */

using DotNetSMPP.Parameter;
using Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetSMPP
{
    public class SMPPClient
    {
        public SequenceProvider seqProvider = new SequenceProvider();

        public int Port { get; set; }
        public string Host { get; set; }
        public string SystemId { get; set; }
        public string Password { get; set; }
        public string SystemType { get; set; }
        public CommnadType Mode { get; set; }
        public InterfaceVersion interfaceVersion = InterfaceVersion.v34;
        public byte source_ton = 0;
        public byte source_npi = 0;
        public byte destination_ton = 0;
        public byte destination_npi = 0;


        private SystemSocket s;
        private List<PDU> outputQueue = new List<PDU>();

        public SMPPClient(string hostname, int port, string username, string password, string systemType, CommnadType bindMode)
        {

            seqProvider = new SequenceProvider();
            this.Host = hostname;
            this.Port = port;
            this.SystemId = username;
            this.Password = password;
            this.SystemType = systemType;
            this.Mode = bindMode;
        }

        public bool Bind()
        {
            s = new SystemSocket(this.Host, this.Port);
            s.packetRecieved += new SystemSocket.PacketRecieved(OnPacketRecived);
            s.Connect();

            var seq = this.seqProvider.GetNumber();
            var sendBytes = GetBindRequestPDU(seq);
            s.Write(sendBytes.ToArray());

            //here wait for response
            return WaitForResponse(seq);
        }

        public bool UnBind()
        {
            //here get bind PUD
            var seq = seqProvider.GetNumber();
            var sendBytes = GetUnbindPDU(seq);
            s.Write(sendBytes.ToArray());
            return WaitForResponse(seq);
        }





        private bool WaitForResponse(int seq , int timeOut = 0)
        {
            var result = false;
            while (true)
            {
                System.Threading.Thread.Sleep(20);
                var p = outputQueue.AsEnumerable().Where(x => x.sequence_number == seq).FirstOrDefault();
                if (p == null) continue;
                outputQueue.Remove(p);
                result = p.command_status.Equals((int)CommandStatus.ESME_ROK);
                break;
            }
            return result;
        }
        public byte[] GetBindRequestPDU(int seq)
        {
            //here get bind PUD
          
            var sendBytes = new List<byte>();
            sendBytes.AddRange(Protocol.ProtocolHelper.GetCommondBytes(this.Mode));
            sendBytes.AddRange(HelperClass.ConvertIntToBytes(0));
            sendBytes.AddRange(HelperClass.ConvertIntToBytes(seq));
            sendBytes.AddRange(HelperClass.ConvertStringToBytes(this.SystemId));
            sendBytes.Add(HelperClass.NullByte);
            sendBytes.AddRange(HelperClass.ConvertStringToBytes(this.Password));
            sendBytes.Add(HelperClass.NullByte);
            sendBytes.AddRange(HelperClass.ConvertStringToBytes(this.SystemType));
            sendBytes.Add(HelperClass.NullByte);
            sendBytes.Add(HelperClass.ConvertIntToBytes(((int)this.interfaceVersion))[3]);
            sendBytes.Add(this.source_ton);
            sendBytes.Add(this.source_npi);
            sendBytes.Add(HelperClass.NullByte);
            sendBytes.InsertRange(0, HelperClass.ConvertIntToBytes(sendBytes.Count + 4));
            return sendBytes.ToArray();
        }
        public byte[] GetUnbindPDU(int seq)
        {
            var sendBytes = new List<byte>();
            sendBytes.AddRange(HelperClass.ConvertIntToBytes((int)CommnadType.unbind));
            sendBytes.AddRange(HelperClass.ConvertIntToBytes(0));
            sendBytes.AddRange(HelperClass.ConvertIntToBytes(seq));
            sendBytes.InsertRange(0, HelperClass.ConvertIntToBytes(sendBytes.Count + 4));
            return sendBytes.ToArray();
        }
        private void OnPacketRecived(object sender, EventArgs e)
        {
            //here get packet and add this to Queue
            //outputQueue.Enqueue(((Packet)e).bytes);
            var pkt = ((EventArgsPacket)e).bytes;
            //here ignore invalid length Packet
            if (pkt.Length < 16)
            {
                Console.WriteLine("Wrong packet received");
                return;
            }

            //var command_length = new byte[4];
            var command_id = new byte[4];
            //var command_status = new byte[4];
            //var sequence_number = new byte[4];
            Array.Copy(pkt, 4, command_id, 0, 4);
            var cmd = Helper.HelperClass.ConvertBytesToInt(command_id);

            switch ((CommnadType)cmd)
            {
                case CommnadType.bind_receiver_resp:
                case CommnadType.bind_transceiver_resp:
                case CommnadType.bind_transmitter_resp:
                case CommnadType.unbind_resp:
                    var p = new BindResponsePDU(pkt);
                    outputQueue.Add(p);
                    break;


                default:
                    Console.WriteLine("Unknown Packet Recieved");
                    break;
            }

        }


    }
}
