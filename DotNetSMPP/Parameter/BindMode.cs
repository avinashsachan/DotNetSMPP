/*

 
 
 */

namespace DotNetSMPP
{
    public enum BindMode : uint
    {
        bind_receiver = 0x00000001,
        bind_transmitter = 0x00000002,
        bind_transceiver = 0x00000009,
    }
}