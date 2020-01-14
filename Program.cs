using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace EPLP
{
    class Program
    {
        static void Main(string[] args)
        {


            EPLP ePLP = new EPLP(new MemoryStream(StringToByteArray(Console.ReadLine())),);
            Console.WriteLine();
        }
        private static byte[] StringToByteArray(String hex)
        {
            int NumberChars = hex.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }
    }
}
