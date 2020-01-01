/*

 
 
 */

using DotNetSMPP.Parameter;
using Helper;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DotNetSMPP
{
    public class SMPPClient
    {
        // Connecction rely in 2 things 
        // 1 : Socket Connectivity
        // 2 : Service Connectivity
        // Let's assume for now this parameter will hold combined result
        // Later we need to handle both state in this parameter
        public bool IsServiceConnected { get; set; }

        public bool IsConnected
        {

            get
            {
                return this._SystemSocket != null & this._SystemSocket.IsConnected && this.IsServiceConnected;
            }

        }


        //this will provide sequnce number for each communication/PDU 
        private SequenceProvider seqProvider = new SequenceProvider();
        //this will provide reference number for "sar_msg_ref_num" TLV parameter in case of mylty part msg
        private ReferenceNumberProvider refProvider = new ReferenceNumberProvider();


        public int Port { get; set; }
        public string Host { get; set; }
        public string SystemId { get; set; }
        public string Password { get; set; }
        public string SystemType { get; set; }


        public SMSTransmisstionMode sMSTransmisstionMode = SMSTransmisstionMode.DATA_MULTIPART;
        public BindMode connectionBindMode { get; private set; }

        public InterfaceVersion interfaceVersion = InterfaceVersion.v34;
        public byte source_ton = 0;
        public byte source_npi = 0;
        public byte destination_ton = 0;
        public byte destination_npi = 0;

        public byte service_type = 0;
        public string source_addr = "";

        //public string destination_addr = "";

        //smpp.esm.submit.msg_mode
        //smpp.esm.submit.msg_type
        //smpp.esm.submit.features
        public byte esm_class = 0;

        //smpp.protocol_id
        public byte protocol_id = 0;


        //smpp.priority_flag
        public byte priority_flag = 0;

        //smpp.schedule_delivery_time_r
        public byte schedule_delivery_time_r = 0;

        //smpp.validity_period_r
        public byte validity_period_r = 0;

        //smpp.regdel.receipt
        //smpp.regdel.acks
        //smpp.regdel.notif

        public byte receipt = 1;

        //smpp.replace_if_present_flag
        public byte replace_if_present_flag = 0;

        //smpp.data_coding
        public byte data_coding = 0;

        //smpp.sm_default_msg_id
        public byte sm_default_msg_id = 0;



        private SystemSocket _SystemSocket;
        private List<PDU> outputQueue = new List<PDU>();

        public SMPPClient(string hostname, int port, string username, string password, string systemType, BindMode bindMode)
        {

            seqProvider = new SequenceProvider();
            this.Host = hostname;
            this.Port = port;
            this.SystemId = username;
            this.Password = password;
            this.SystemType = systemType;
            this.connectionBindMode = bindMode;
        }


        public delegate void EnsureLink(SMPPClient s);

        public bool Bind()
        {
            _SystemSocket = new SystemSocket(this.Host, this.Port);
            _SystemSocket.packetRecieved += new SystemSocket.PacketRecieved(OnPacketRecived);
            var connected = _SystemSocket.Connect();
            if (!connected)
            {
                //safe side
                _SystemSocket.Disconnect();
                throw new Exception("Failed to connect");
            }

            // here actually service is not connected , but we are setting this to true
            // because our wait for Response method uses this :) 
            IsServiceConnected = true;

            this.seqProvider.Reset();

            var seq = this.seqProvider.GetNumber();
            var sendBytes = GetBindRequestPDU(seq);
            _SystemSocket.Write(sendBytes.ToArray());

            //here wait for response
            PDU p = null;
            var result = WaitForResponse(seq, out p);
            if (p != null && result)
            {
                //here we need to add one more thread to Ensure Link
                var ensureLink = new EnsureLink(CheckLink);
                ensureLink.BeginInvoke(this, CheckLinkResult, this);
            }
            else
            {
                IsServiceConnected = false;
            }

            return result;

        }

        private void CheckLinkResult(IAsyncResult ar)
        {
            //throw new NotImplementedException();
            var s = (SMPPClient)ar.AsyncState;
            try
            {
                IsServiceConnected = false;

                //Here actually socket is disconnected but send disconnect
                s._SystemSocket.Disconnect();
            }
            catch (Exception ex)
            {

            }
        }

        private void CheckLink(SMPPClient s)
        {
            System.Threading.Thread.Sleep(15 * 1000);
            //throw new NotImplementedException();
            while (s.IsConnected)
            {
                var seq = s.seqProvider.GetNumber();

                //here build ensure link packet
                var sendByte = new List<byte>();
                sendByte.AddRange(HelperClass.ConvertIntToBytes(16));
                sendByte.AddRange(HelperClass.ConvertIntToBytes((uint)CommanadType.enquire_link));
                sendByte.AddRange(HelperClass.ConvertIntToBytes(0));
                sendByte.AddRange(HelperClass.ConvertIntToBytes(seq));
                s._SystemSocket.Write(sendByte.ToArray());
                System.Threading.Thread.Sleep(15 * 1000);
            }
        }

        public bool UnBind()
        {
            if (!this._SystemSocket.IsConnected) return true;

            //here get bind PUD
            var seq = seqProvider.GetNumber();
            var sendBytes = GetUnbindPDU(seq);
            var i = _SystemSocket.Write(sendBytes.ToArray());
            if (i == -1)
            {
                IsServiceConnected = false;
                return true;
            }


            PDU p = null;
            var result = WaitForResponse(seq, out p);

            if (result)
            {
                IsServiceConnected = false;
                _SystemSocket.Disconnect();
            }

            return result;
        }



        private bool WaitForResponse(int seq, out PDU pdu, int timeOut = 0)
        {
            var result = false;
            pdu = null;
            while (true)
            {
                //TODO: here check if connection is disconnected
                if (!this.IsConnected) break;

                System.Threading.Thread.Sleep(20);
                var p = outputQueue.AsEnumerable().Where(x => x.sequence_number == seq).FirstOrDefault();
                if (p == null) continue;
                outputQueue.Remove(p);
                pdu = p;
                result = p.command_status.Equals(CommandStatus.ESME_ROK);
                break;
            }
            return result;
        }
        public byte[] GetBindRequestPDU(int seq)
        {
            //here get bind PUD

            var sendBytes = new List<byte>();
       
            sendBytes.AddRange(HelperClass.ConvertIntToBytes((uint)this.connectionBindMode));
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
            sendBytes.AddRange(HelperClass.ConvertIntToBytes((int)CommanadType.unbind));
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

            Console.WriteLine("Packet Recieved : {0}", (CommanadType)cmd);

            switch ((CommanadType)cmd)
            {
                case CommanadType.bind_receiver_resp:
                case CommanadType.bind_transceiver_resp:
                case CommanadType.bind_transmitter_resp:
                case CommanadType.unbind_resp:
                    var p = new BindResponsePDU(pkt);
                    outputQueue.Add(p);
                    break;
                case CommanadType.submit_sm_resp:
                case CommanadType.data_sm_resp:

                    var p1 = new SubmitSMRespPDU(pkt);
                    outputQueue.Add(p1);
                    break;

                case CommanadType.deliver_sm:
                    var dsr = new UnknownPDU(pkt);
                    outputQueue.Add(dsr);

                    SendResponse(CommanadType.deliver_sm_resp, dsr.sequence_number, new byte[] { 0 });


                    break;

                case CommanadType.enquire_link_resp:
                    var x123 = new UnknownPDU(pkt);

                    //here check status if it is NoK , Need to raise this
                    if (x123.command_status != CommandStatus.ESME_ROK)
                    {
                        UnBind();
                    }

                    break;
                case CommanadType.enquire_link:
                    var x234 = new UnknownPDU(pkt);
                    SendResponse(CommanadType.enquire_link_resp, x234.sequence_number, new byte[] { });
                    break;
                default:
                    //here send NACK
                    var x = new UnknownPDU(pkt);
                    outputQueue.Add(x);
                    Console.WriteLine("Unknown Packet Recieved {0}", x.command_id.ToString());
                    SendResponse(CommanadType.generic_nack, x.sequence_number, new byte[] { });
                    break;
            }

        }

        private void SendResponse(CommanadType commnadType, int sequence_number, byte[] payLoad)
        {
            //
            var sendbyte = new List<byte>();
            //sendbyte.AddRange(HelperClass.ConvertIntToBytes(16));

            sendbyte.AddRange(HelperClass.ConvertIntToBytes((uint)commnadType));
            sendbyte.AddRange(HelperClass.ConvertIntToBytes((uint)CommandStatus.ESME_ROK));
            sendbyte.AddRange(HelperClass.ConvertIntToBytes(sequence_number));
            sendbyte.AddRange(payLoad);

            sendbyte.InsertRange(0, HelperClass.ConvertIntToBytes(sendbyte.Count + 4));
            _SystemSocket.Write(sendbyte.ToArray());
        }

        ///single msg
        public bool SubmitSm(string sourceAddr, string distinationAddr, string msg)
        {
            //here form PDU
            var seq = seqProvider.GetNumber();

            var sendBytes = new List<byte>();
            sendBytes.AddRange(HelperClass.ConvertIntToBytes((int)CommanadType.submit_sm));
            sendBytes.AddRange(HelperClass.ConvertIntToBytes(0));
            sendBytes.AddRange(HelperClass.ConvertIntToBytes(seq));

            //
            sendBytes.Add(service_type);
            sendBytes.Add(source_ton);
            sendBytes.Add(source_npi);

            sendBytes.AddRange(HelperClass.ConvertStringToBytes(string.IsNullOrEmpty(sourceAddr) ? source_addr : sourceAddr));
            sendBytes.Add(HelperClass.NullByte);

            sendBytes.Add(destination_ton);
            sendBytes.Add(destination_npi);
            sendBytes.AddRange(HelperClass.ConvertStringToBytes(distinationAddr));
            sendBytes.Add(HelperClass.NullByte);

            sendBytes.Add(esm_class);
            sendBytes.Add(protocol_id);
            sendBytes.Add(priority_flag);

            sendBytes.Add(schedule_delivery_time_r);
            sendBytes.Add(validity_period_r);
            sendBytes.Add(receipt);
            sendBytes.Add(replace_if_present_flag);
            sendBytes.Add(data_coding);
            sendBytes.Add(sm_default_msg_id);


            //here add msg
            var msgBytes = HelperClass.ConvertStringToBytes(msg);
            //sendBytes.Add((byte)msgBytes.Length);
            //sendBytes.AddRange(msgBytes);

            //msg length
            sendBytes.Add((byte)0);

            //add tlv 
            sendBytes.AddRange(HelperClass.ConvertIntToBytes((ushort)TLVTag.message_payload));
            sendBytes.AddRange(HelperClass.ConvertIntToBytes((ushort)msgBytes.Length));
            sendBytes.AddRange(msgBytes);


            sendBytes.InsertRange(0, HelperClass.ConvertIntToBytes(sendBytes.Count + 4));
            _SystemSocket.Write(sendBytes.ToArray());

            PDU p = null;
            var r = WaitForResponse(seq, out p);
            Console.WriteLine(((SubmitSMRespPDU)p).message_id);

            return r;
        }

        //data msg
        public bool DataSm(string sourceAddr, string distinationAddr, string msg)
        {
            //here form PDU
            var seq = seqProvider.GetNumber();

            var sendBytes = new List<byte>();
            sendBytes.AddRange(HelperClass.ConvertIntToBytes((int)CommanadType.data_sm));
            sendBytes.AddRange(HelperClass.ConvertIntToBytes(0));
            sendBytes.AddRange(HelperClass.ConvertIntToBytes(seq));

            //
            sendBytes.Add(service_type);
            sendBytes.Add(source_ton);
            sendBytes.Add(source_npi);

            sendBytes.AddRange(HelperClass.ConvertStringToBytes(string.IsNullOrEmpty(sourceAddr) ? source_addr : sourceAddr));
            sendBytes.Add(HelperClass.NullByte);

            sendBytes.Add(destination_ton);
            sendBytes.Add(destination_npi);
            sendBytes.AddRange(HelperClass.ConvertStringToBytes(distinationAddr));
            sendBytes.Add(HelperClass.NullByte);

            sendBytes.Add(esm_class);
            sendBytes.Add(receipt);
            sendBytes.Add(data_coding);



            //here add msg
            var msgBytes = HelperClass.ConvertStringToBytes(msg);

            //add tlv 
            sendBytes.AddRange(HelperClass.ConvertIntToBytes((ushort)TLVTag.message_payload));
            sendBytes.AddRange(HelperClass.ConvertIntToBytes((ushort)msgBytes.Length));
            sendBytes.AddRange(msgBytes);


            sendBytes.InsertRange(0, HelperClass.ConvertIntToBytes(sendBytes.Count + 4));
            _SystemSocket.Write(sendBytes.ToArray());

            PDU p = null;
            var r = WaitForResponse(seq, out p);
            if (p == null)
            {
                Console.WriteLine(((SubmitSMRespPDU)p).message_id);
            }
            return r;
        }

        ///Submit SM Multi
        private SubmitSMRespPDU DataSm_MultiPart(string sourceAddr, string distinationAddr, string msg, int totalParts, int partIndex)
        {
            //here form PDU
            var seq = seqProvider.GetNumber();

            var refNum = refProvider.GetNumber();

            var sendBytes = new List<byte>();
            sendBytes.AddRange(HelperClass.ConvertIntToBytes((int)CommanadType.data_sm));
            sendBytes.AddRange(HelperClass.ConvertIntToBytes(0));
            sendBytes.AddRange(HelperClass.ConvertIntToBytes(seq));

            //
            sendBytes.Add(service_type);
            sendBytes.Add(source_ton);
            sendBytes.Add(source_npi);

            sendBytes.AddRange(HelperClass.ConvertStringToBytes(string.IsNullOrEmpty(sourceAddr) ? source_addr : sourceAddr));
            sendBytes.Add(HelperClass.NullByte);

            sendBytes.Add(destination_ton);
            sendBytes.Add(destination_npi);
            sendBytes.AddRange(HelperClass.ConvertStringToBytes(distinationAddr));
            sendBytes.Add(HelperClass.NullByte);

            sendBytes.Add(esm_class);

            sendBytes.Add(receipt);

            sendBytes.Add(data_coding);


            //here add msg
            var msgBytes = HelperClass.ConvertStringToBytes(msg);


            //add tlv 
            sendBytes.AddRange(HelperClass.ConvertIntToBytes((ushort)TLVTag.message_payload));
            sendBytes.AddRange(HelperClass.ConvertIntToBytes((ushort)msgBytes.Length));
            sendBytes.AddRange(msgBytes);


            sendBytes.AddRange(HelperClass.ConvertIntToBytes((ushort)TLVTag.sar_msg_ref_num));
            sendBytes.AddRange(HelperClass.ConvertIntToBytes((ushort)2));
            sendBytes.AddRange(HelperClass.ConvertIntToBytes((ushort)refNum));

            sendBytes.AddRange(HelperClass.ConvertIntToBytes((ushort)TLVTag.sar_total_segments));
            sendBytes.AddRange(HelperClass.ConvertIntToBytes((ushort)1));
            sendBytes.Add((byte)totalParts);

            sendBytes.AddRange(HelperClass.ConvertIntToBytes((ushort)TLVTag.sar_segment_seqnum));
            sendBytes.AddRange(HelperClass.ConvertIntToBytes((ushort)1));
            sendBytes.Add((byte)partIndex);



            sendBytes.InsertRange(0, HelperClass.ConvertIntToBytes(sendBytes.Count + 4));
            _SystemSocket.Write(sendBytes.ToArray());

            PDU p = null;
            var r = WaitForResponse(seq, out p);
            //Console.WriteLine(().message_id);
            return p != null ? (SubmitSMRespPDU)p : null;
        }



    }
}
