using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetSMPP
{
    public class BindResponsePDU : PDU
    {
      
        public BindResponsePDU(byte[] pkt)
        {
            var command_length = new byte[4];
            var command_id = new byte[4];
            var command_status = new byte[4];
            var sequence_number = new byte[4];

            Array.Copy(pkt, 0, command_length, 0, 4);
            Array.Copy(pkt, 4, command_id, 0, 4);
            Array.Copy(pkt, 8, command_status, 0, 4);
            Array.Copy(pkt, 12, sequence_number, 0, 4);

            this.command_length = Helper.HelperClass.ConvertBytesToInt(command_id);
            this.command_id = (CommnadType)Helper.HelperClass.ConvertBytesToInt(command_id);
            this.command_status = Helper.HelperClass.ConvertBytesToInt(command_status);
            this.sequence_number = Helper.HelperClass.ConvertBytesToInt(sequence_number);
        }


        public byte[] GetBytes()
        {



            return null;
        }

    }
    
}
