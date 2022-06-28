using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CShidori.DataHandler;
using CShidori.Core;

namespace CShidori.Core
{
    public static class Mutation
    {
         
        public static string RepRandBc(Random rand, string p)
        {         
            int randvalue = rand.Next(p.Length);
            int randbc = rand.Next(BadStrings.Output.Count);  
            
            StringBuilder sb = new StringBuilder(p);
            sb.Remove(randvalue, 1);     
            
            return sb.ToString().Insert(randvalue, BadStrings.Output[randbc]);
        }


        public static string AddRandBc( Random rand, string p)
        {
            int randvalue = rand.Next(p.Length);
            int randbc = rand.Next(BadStrings.Output.Count);

            return p.Insert(randvalue, BadStrings.Output[randbc]);
        }

        public static string RepLine(Random rand, string p)
        {
            string[] lines = p.Split('\n');
            int randvalue = rand.Next(lines.Length);
            int randbc = rand.Next(BadStrings.Output.Count);
            lines[randvalue] = BadStrings.Output[randbc];

            return String.Join('\n', lines);
        }


        public static string DelChar( Random rand, string p)
        {
            int randvalue = rand.Next(p.Length);
            StringBuilder sb = new StringBuilder(p);
            
            return sb.Remove(randvalue, 1).ToString();
        }

        public static string BitFlip(Random rand, string p)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(p);
            byte[] bitW = { 0x01, 0x02, 0x04, 0x08, 0x10, 0x20, 0x40, 0x80 };

            int randvalue = rand.Next(bytes.Length);
            int randbit = rand.Next(bitW.Length);

            try { bytes[randvalue] += bitW[randbit]; }
            catch { bytes[randvalue] -= bitW[randbit]; }

            return Encoding.UTF8.GetString(bytes);
        }


        public static string RepThreeBytes(Random rand, string p)
        {
            if(p.Length < 3){ p += Misc.RandomString(3); }

            byte[] bytes = Encoding.UTF8.GetBytes(p);
            byte[] ByteRange = new Byte[3];

            int randvalue = rand.Next(bytes.Length - 2);
            rand.NextBytes(ByteRange);

            bytes[randvalue] = ByteRange[0];
            bytes[randvalue+1] = ByteRange[1];
            bytes[randvalue + 2] = ByteRange[2];

            return Encoding.UTF8.GetString(bytes);
        }

        public static string RepeatStr(Random rand, string p)
        {

            string[] lines = p.Split('\n');
            int randvalue = rand.Next(lines.Length);
            int randbc = rand.Next(BadStrings.Output.Count);
            StringBuilder sb = new StringBuilder(lines[randvalue]);
            lines[randvalue] = sb.ToString(rand.Next(sb.Length), rand.Next(sb.Length));

            return String.Join('\n', lines);
        }


    }
}
