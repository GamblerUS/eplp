using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace EPLP
{
    class EPLP
    {
        private List<byte[,]> Parts = new List<byte[,]>();
        private byte Dimensions;
        public float EntropyThreshold { get; private set; }
        public ushort Passes { get; private set; }
        public EPLP(float EntropyThreshold, ushort Passes)
        {
            this.EntropyThreshold = EntropyThreshold;
            this.Passes = Passes;

        }
        public Stream FromRaw(Stream Stream)
        {
            return Process(Stream);
            Parts.Add(Stream);
        }
        private byte Deviation(byte[,] Stream)
        {
            byte[] Buffer = new byte[Stream.Length];
            Stream.Read(Buffer, 0, (int)Stream.Length);
            byte Mean_ = Mean(Stream);
            byte Deviation;
            if (Buffer.Count() > 0)
            {
                //Perform the Sum of (value-avg)_2_2      
                ulong Sum = (byte)Buffer.Sum(d => Math.Pow(d - Mean_, 2));
                //Put it all together      
                Deviation = (byte)Math.Sqrt(Sum / ((ulong)Buffer.Count() - 1));
                return Deviation;
            }
            else
            {
                return 0;
            }
        }
        private byte Mean(Stream Stream)
        {
            byte[] Buffer = new byte[Stream.Length];
            Stream.Read(Buffer, 0, (int)Stream.Length);
            ulong Sum = 0;
            foreach (byte b in Buffer)
            {
                Sum += b;
            }
            return (byte)(Sum / (ulong)Stream.Length);
        }
        private Stream Process(byte[,] Stream)
        {
            for (ulong i = 0; i < Passes; i++)
            {
                ulong a = 0;
                foreach (byte[,] Part in Parts)
                {                    
                    if (Deviation(Part) > 0xff*EntropyThreshold)
                    {
                    //divide stream into 2 words
                        byte[,] DividedContainer = DividedStream(Part);
                        //replace currently loaded word with the halved word, advance index a by 2
                        Parts[(int)a] = DividedContainer[0];
                        Parts.Insert((int)a + 1, DividedContainer[1]);
                        a += 2;
                    }
                    else
                    {
                        a++;
                    }
                }
            }
            Stream ConcatStream = new MemoryStream();
            foreach (Stream Part in Parts)
            {
                byte[] b = new byte[Part.Length];
                for (ulong i = 0; i < (ulong)Part.Length; i++)
                {
                    b[i] = Mean(Part);
                }
                ConcatStream.Write(b,0,(int)Part.Length);
            }
            return ConcatStream;
        }
        public byte[,][] DividedStream(byte[,] Stream)
        {
            //find half the stream's length
            float HalfLength = (ulong)Math.Round((float)(Stream.Length / 2));
            //in case of unwhole stream halflength, floor and ceil into an array
            ulong[] HalfLengths = new ulong[2]
            {
                (ulong)Math.Ceiling(HalfLength),
                (ulong)Math.Floor(HalfLength)
            };
            byte[,][] TempBuffer = new byte[3,3][];
            byte[,] aBuffer = new List<byte>(Stream).GetRange(2, 2).ToArray(); 
            byte[,] bBuffer = new byte[HalfLengths[1]][];
            
            aBuffer = 
            TempBuffer[][0] = new MemoryStream(aBuffer);
            TempBuffer[][1] = new MemoryStream(bBuffer);
            return TempBuffer;
        }
    }

    }
// 2019 Hunter Wheat 
