using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CShidori
{
    public class Mutation
    {
        public string Input { get; set; }
        public List<string> Output { set; get; }

        public Mutation(int n, string param ) 
        {
            this.Input = param;
            this.Output = new List<string>();

            while(n >= 1)
            {

                this.Output.Add(randombc());
                this.Output.Add(bitflip());
                n -= 1;
            }
        }
        
        public string randombc()
        {

            string result = string.Empty;
            var rand = new Random();
            List<string> bc = new BadChars().Output;
            int randvalue = rand.Next(this.Input.Length);
            int randbc = rand.Next(bc.Count -1);         
            //Console.WriteLine("randvalue {0}, randbc: {1}", randvalue, randbc);

            StringBuilder sb = new StringBuilder(this.Input);
            sb.Remove(randvalue, 1);
            result = sb.ToString();

            if(randvalue < 0)
            {
                randvalue -= 1;
            }
            
            return result.Insert(randvalue, bc[randbc]);

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
