using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetSMPP
{
    public class SubmitSMRespPDU : PDU
    {
        //smpp.message_id
        public string message_id = "";


        public SubmitSMRespPDU(byte[] pkt)
        {
            var command_length = new byte[4];
            var command_id = new byte[4];
            var command_status = new byte[4];
            var sequence_number = new byte[4];
            var msgId = new byte[pkt.Length - 16 - 1];

            Array.Copy(pkt, 0, command_length, 0, 4);
            Array.Copy(pkt, 4, command_id, 0, 4);
            Array.Copy(pkt, 8, command_status, 0, 4);
            Array.Copy(pkt, 12, sequence_number, 0, 4);
            Array.Copy(pkt, 16, msgId, 0, msgId.Length);


            this.command_length = Helper.HelperClass.ConvertBytesToInt(command_id);
            this.command_id = (CommanadType)Helper.HelperClass.ConvertBytesToInt(command_id);
            this.command_status = (CommandStatus)Helper.HelperClass.ConvertBytesToInt(command_status);
            this.sequence_number = Helper.HelperClass.ConvertBytesToInt(sequence_number);
            this.message_id = Helper.HelperClass.ConvertBytesToString(msgId);
        }
    }
}
