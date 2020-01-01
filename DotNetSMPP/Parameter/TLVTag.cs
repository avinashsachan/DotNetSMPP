using System;
using System.ComponentModel;

namespace DotNetSMPP.Parameter
{
    enum TLVTag : ushort
    {

        [Description("SMSC handle of the message being receipted.")]
        receipted_message_id = 0x001E,

        [Description("Unique reference ID for multipart message ( 0x0000-0xFFFF )")]
        sar_msg_ref_num = 0x020C,

        [Description("Total number of parts used for message")]
        sar_total_segments = 0x020E,

        [Description("Part number of multipart message")]
        sar_segment_seqnum = 0x020F,


        network_error_code = 0x0423,

        [Description("The message_payload parameter contains the user data.")]
        message_payload = 0x0424,

        delivery_failure_reason = 0x0425,

        [Description("Final message state for an SMSC Delivery Receipt.")]
        message_state = 0x0427,
    }

    
}
