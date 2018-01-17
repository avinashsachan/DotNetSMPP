namespace DotNetSMPP.Parameter
{
    enum TLVTag : ushort
    {
        sar_total_segments = 0x020E,
        sar_segment_seqnum = 0x020F,
        message_payload = 0x0424,
        message_state = 0x0427,
    }
}
