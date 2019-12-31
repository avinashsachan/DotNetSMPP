using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetSMPP
{
    public static class Util
    {
        public static byte[] ConvertToByteArray(string strText)
        {
            return strText.ToCharArray().Select(x => Convert.ToByte(x)).ToArray();
        }


        public static string ConvertToString(byte[] bytes)
        {
            return string.Join(string.Empty, bytes.Select(x => Convert.ToChar(x)).ToArray());
           
        }
    }
}
