using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using CShidori.DataHandler;
using CShidori.Core;

namespace CShidori.Core
{
    public static class Mutation
    {
         
        public static string RepRandBc(string p)
        {         
            int randvalue = RandomNumberGenerator.GetInt32(p.Length);
            int randbc = RandomNumberGenerator.GetInt32(BadStrings.Output.Count);  
            
            StringBuilder sb = new StringBuilder(p);
            sb.Remove(randvalue, 1);     
            
            return sb.ToString().Insert(randvalue, BadStrings.Output[randbc]);
        }


        public static string AddRandBc(string p)
        {
            int randvalue = RandomNumberGenerator.GetInt32(p.Length);
            int randbc = RandomNumberGenerator.GetInt32(BadStrings.Output.Count);

            return p.Insert(randvalue, BadStrings.Output[randbc]);
        }

        public static string RepLine(string p)
        {
            string[] lines = p.Split('\n');
            int randvalue = RandomNumberGenerator.GetInt32(lines.Length);
            int randbc = RandomNumberGenerator.GetInt32(BadStrings.Output.Count);
            lines[randvalue] = BadStrings.Output[randbc];

            return String.Join('\n', lines);
        }


        public static string DelChar(string p)
        {
            int randvalue = RandomNumberGenerator.GetInt32(p.Length);
            StringBuilder sb = new StringBuilder(p);
            
            return sb.Remove(randvalue, 1).ToString();
        }

        public static string BitFlip(string p)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(p);
            byte[] bitW = { 0x01, 0x02, 0x04, 0x08, 0x10, 0x20, 0x40, 0x80 };

            int randvalue = RandomNumberGenerator.GetInt32(bytes.Length);
            int randbit = RandomNumberGenerator.GetInt32(bitW.Length);

            try { bytes[randvalue] += bitW[randbit]; }
            catch { bytes[randvalue] -= bitW[randbit]; }

            return Encoding.UTF8.GetString(bytes);
        }


        public static string RepThreeBytes(string p)
        {
            if(p.Length < 3){ p += Misc.RandomString(3); }

            byte[] bytes = Encoding.UTF8.GetBytes(p);
            byte[] ByteRange = new Byte[3];

            int randvalue = RandomNumberGenerator.GetInt32(bytes.Length - 2);
            RandomNumberGenerator.Create().GetBytes(ByteRange);

            bytes[randvalue] = ByteRange[0];
            bytes[randvalue+1] = ByteRange[1];
            bytes[randvalue + 2] = ByteRange[2];

            return Encoding.UTF8.GetString(bytes);
        }

        public static string RepeatStr(string p)
        {

            StringBuilder sb = new StringBuilder(p);
            int startIndex = RandomNumberGenerator.GetInt32(sb.Length);
            
            return p + sb.ToString(startIndex, sb.Length - startIndex);
           
        }


    }
}
