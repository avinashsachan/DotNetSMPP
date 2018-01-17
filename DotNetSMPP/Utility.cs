using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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


    }
}
