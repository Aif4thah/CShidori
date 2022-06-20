using System;
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

        public Mutation(int n, string param, string d ) 
        {
            this.Input = param;
            this.Output = new List<string>();
            List<string> bss = new BadStrings(d).Output;
            if (bss.Count == 0) { bss = new BadStrings("s").Output; }

            if (this.Input != string.Empty)
            {
                while (n >= 1)
                {
                    n -= 1;
                    var rand = new Random();
                    int r = rand.Next(0,10);
                    //Console.WriteLine(r);
                    switch(r)
                    {
                        case 0:
                            this.Output.Add( BitFlip(rand) );
                            break;
                        case 1:
                            this.Output.Add(AddRandBc(bss, rand));
                            break;
                        case 2:
                            this.Output.Add(DelChar(rand));
                            break;
                        case 3:
                            this.Output.Add(RepThreeBytes(rand));
                            break;
                        case > 3:
                            this.Output.Add(RepRandBc(bss, rand));
                            break;

                    }
                                 
                }
            }
        }
        
        private string RepRandBc(List<string> bss, Random rand)
        {         
            int randvalue = rand.Next(this.Input.Length);
            int randbc = rand.Next(bss.Count -1);  
            
            StringBuilder sb = new StringBuilder(this.Input);
            sb.Remove(randvalue, 1);     
            
            return sb.ToString().Insert(randvalue, bss[randbc]);
        }


        private string AddRandBc(List<string> bss, Random rand)
        {
            int randvalue = rand.Next(this.Input.Length);
            int randbc = rand.Next(bss.Count - 1);

            return this.Input.Insert(randvalue, bss[randbc]);
        }


        private string DelChar( Random rand)
        {
            int randvalue = rand.Next(this.Input.Length);

            StringBuilder sb = new StringBuilder(this.Input);
            sb.Remove(randvalue, 1);

            return sb.ToString();
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
