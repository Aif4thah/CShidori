﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CShidori.DataHandler;

namespace CShidori.Core
{
    public class Mutation
    {
        public string Input { get; set; }
        public List<string> Output { set; get; }

        public Mutation(int n, string p) 
        {
            this.Input = p;
            this.Output = new List<string>();

            if (this.Input != string.Empty)
            {
                while (n > 0)
                {
                    n -= 1;
                    var rand = new Random();
                    int r = rand.Next(0,10);

                    switch(r)
                    {
                        case 0:
                            this.Output.Add( BitFlip(rand) );
                            break;

                        case 1:
                            this.Output.Add(AddRandBc(rand));
                            break;

                        case 2:
                            this.Output.Add(DelChar(rand));
                            break;

                        case 3:
                            this.Output.Add(RepThreeBytes(rand));
                            break;

                        case 4:
                            this.Output.Add(RepLine(rand));
                            break;

                        default:
                            this.Output.Add(RepRandBc(rand));
                            break;

                    }


                }
            }
        }
        
        private string RepRandBc(Random rand)
        {         
            int randvalue = rand.Next(this.Input.Length);
            int randbc = rand.Next(BadStrings.Output.Count);  
            
            StringBuilder sb = new StringBuilder(this.Input);
            sb.Remove(randvalue, 1);     
            
            return sb.ToString().Insert(randvalue, BadStrings.Output[randbc]);
        }


        private string AddRandBc( Random rand)
        {
            int randvalue = rand.Next(this.Input.Length);
            int randbc = rand.Next(BadStrings.Output.Count);

            return this.Input.Insert(randvalue, BadStrings.Output[randbc]);
        }

        private string RepLine(Random rand)
        {
            string[] lines = this.Input.Split('\n');
            int randvalue = rand.Next(lines.Length);
            int randbc = rand.Next(BadStrings.Output.Count);

            lines[randvalue] = (rand.Next(2) == 1) ? BadStrings.Output[randbc] : BadStrings.Output[randbc] + "\\n";

            return String.Join('\n', lines);
        }


        private string DelChar( Random rand)
        {
            int randvalue = rand.Next(this.Input.Length);

            StringBuilder sb = new StringBuilder(this.Input);
            
            return sb.Remove(randvalue, 1).ToString();
        }

        private string BitFlip(Random rand)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(this.Input);
            byte[] bitW = { 0x01, 0x02, 0x04, 0x08, 0x10, 0x20, 0x40, 0x80 };

            int randvalue = rand.Next(bytes.Length);
            int randbit = rand.Next(bitW.Length);

            try { bytes[randvalue] += bitW[randbit]; }
            catch { bytes[randvalue] -= bitW[randbit]; }

            return Encoding.UTF8.GetString(bytes);
        }


        public string RepThreeBytes(Random rand)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(this.Input);
            byte[] ByteRange = new Byte[3];

            int randvalue = rand.Next(bytes.Length - 2);
            rand.NextBytes(ByteRange);

            bytes[randvalue] = ByteRange[0];
            bytes[randvalue+1] = ByteRange[1];
            bytes[randvalue + 2] = ByteRange[2];

            return Encoding.UTF8.GetString(bytes);
        }

    }
}