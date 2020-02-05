using System;
using System.Linq;

namespace DotNetSMPP
{
    public static class Utility
    {

        public static string[] GetStringChunks(this string value, int chunkSize)
        {
            if (string.IsNullOrEmpty(value)) throw new ArgumentException("The string cannot be null.");
            if (chunkSize < 1) throw new ArgumentException("The chunk size should be equal or greater than one.");

            int remainder;
            int divResult = Math.DivRem(value.Length, chunkSize, out remainder);

            int numberOfChunks = remainder > 0 ? divResult + 1 : divResult;
            var result = new string[numberOfChunks];

            int i = 0;
            // avoiding last chunk
            while (i < numberOfChunks - 1)
            {
                result[i] = value.Substring(i * chunkSize, chunkSize);
                i++;
            }

            int lastChunkSize = remainder > 0 ? remainder : chunkSize;
            result[i] = value.Substring(i * chunkSize, lastChunkSize);

            return result;
        }

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
