using System;
using System.Collections.Generic;
using System.Web;
using System.Linq;

namespace CShidori.Core 
{
    public class XssInjection
    {
        public string Input { get; set; }

        public XssInjection(string param)
        {
            
            this.Input = param;

            List<string> results = new List<string>();
            List<string> tmp = new List<string>();
            List<string> wrapped = new List<string>();

            wrapped.AddRange(wrapp(this.Input));
            results.AddRange(wrapped);

            results = EncodeStrings.encodebadchars(results); //results.Distinct().ToList(); is performed here

            Console.WriteLine(String.Join("\n", results));

        }

        private List<string> wrapp(string param)
        {
            List<string> results = new List<string>();
            List<string> lines = new DataLoader().Dataloader("JavaScript");
            foreach (string line in lines)
            {
                results.Add(
                    line.Replace("§",param));
            }

            return results;
        }






    }
}
