using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetSMPP.Protocol
{
    public class ProtocolHelper
    {

        public static byte[] GetCommondBytes(CommnadType cmd)
        {
            var b = BitConverter.GetBytes((uint)cmd);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(b);
            }
            return b.ToArray();
        }


        public static CommnadType GetCommondFromBytes(byte[] b)
        {
            if (BitConverter.IsLittleEndian) {
                Array.Reverse(b);
            }
            var i = BitConverter.ToUInt32(b,0);
            return (CommnadType)i;
        }
    }
}
