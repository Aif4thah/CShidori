using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CShidori.Core
{
    public class Mutation
    {
        public string Input { get; set; }
        public List<string> Output { set; get; }

        public Mutation(int n, string param ) 
        {
            this.Input = param;
            this.Output = new List<string>();

            if (this.Input != string.Empty)
            {
                while (n >= 1)
                {
                    this.Output.Add(randombc());
                    this.Output.Add(bitflip());
                    n -= 1;
                }
            }

        }
        
        public string randombc()
        {

            string result = string.Empty;
            var rand = new Random();
            List<string> bss = new BadStrings().Output;
            int randvalue = rand.Next(this.Input.Length);
            int randbc = rand.Next(bss.Count -1);         
            //Console.WriteLine("randvalue {0}, randbc: {1}, Input: {2}", randvalue, randbc, this.Input);

            StringBuilder sb = new StringBuilder(this.Input);
            sb.Remove(randvalue, 1);
            result = sb.ToString().Insert(randvalue, bss[randbc]);


             result = bss[randbc];

           
            return result;

        }

        public string bitflip()
        {

            byte[] bytes = Encoding.UTF8.GetBytes(this.Input);
            byte[] biteW = { 0x01, 0x02, 0x04, 0x08, 0x10, 0x20, 0x40, 0x80 };
            var rand = new Random();
            int randvalue = rand.Next(bytes.Length);
            int randbit = rand.Next(biteW.Length);

            try
            {
                bytes[randvalue] += biteW[randbit];
            }
            catch
            {
                bytes[randvalue] -= biteW[randbit];
            }

            return Encoding.UTF8.GetString(bytes) ;
        }
    }
}
