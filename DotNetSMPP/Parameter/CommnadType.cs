namespace DotNetSMPP
{
    public enum CommnadType : uint
    {


        generic_nack = 0x80000000,

        bind_receiver = 0x00000001,
        bind_receiver_resp = 0x80000001,
        bind_transmitter = 0x00000002,
        bind_transmitter_resp = 0x80000002,

        submit_sm = 0x00000004,
        submit_sm_resp = 0x80000004,

        deliver_sm = 0x00000005,
        deliver_sm_resp = 0x80000005,

        unbind = 0x00000006,
        unbind_resp = 0x80000006,

        bind_transceiver = 0x00000009,
        bind_transceiver_resp = 0x80000009,

        enquire_link = 0x00000015,
        enquire_link_resp = 0x80000015,
    }
}
