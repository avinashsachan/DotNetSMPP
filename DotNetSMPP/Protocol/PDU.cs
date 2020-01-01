using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetSMPP
{
    public abstract class PDU
    {
        public int command_length { get; set; }
        public CommanadType command_id { get; set; }
        public CommandStatus command_status { get; set; }
        public int sequence_number { get; set; }
        
    }
}
