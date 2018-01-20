using System;
using System.Linq;

namespace Helper
{
    public class HelperClass
    {

        public static byte NullByte = 0;


        public static byte[] ConvertIntToBytes(ushort input)
        {
            var p = BitConverter.GetBytes(input);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(p);
            }
            return p;
        }

        public static byte[] ConvertIntToBytes(Int32 input)
        {
            var p = BitConverter.GetBytes(input);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(p);
            }
            return p;
        }

        public static byte[] ConvertIntToBytes(uint input)
        {
            var p = BitConverter.GetBytes(input);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(p);
            }
            return p;
        }

        public static int ConvertBytesToInt(byte[] input)
        {
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(input);
            }
            return BitConverter.ToInt32(input, 0);
        }




        public static byte[] ConvertStringToBytes(string input)
        {
            if (string.IsNullOrEmpty(input)) return new byte[] { };
            return input.AsEnumerable().Select(x => Convert.ToByte(x)).ToArray();
        }



        public static string ConvertBytesToString(byte[] bt)
        {
            if (bt == null || bt.Length == 0) return string.Empty;
            return string.Join("", bt.AsEnumerable().Select(x => Convert.ToChar(x)));
        }



    }
}
