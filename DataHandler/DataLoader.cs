using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CShidori.DataHandler
{
    internal class DataLoader
    {

        public List<string> Dataloader(string d)
        {
            string[] lines;
            List<string> results = new List<string>();
            List<string> FileList = new List<string>()
            {
                "Data/BadChars",
                "Data/BadStrings",
                "Data/Java",
                "Data/DotNet",
                "Data/JavaScript",
                "Data/CsrfTemplate",
                "Data/MsTestTemplate",
                "Data/XxeTemplate",
                "Data/C",
                "Data/Angular"

            };


            foreach( string s in d.Split(","))
            {
                //Console.WriteLine("DataLoader: Linq resquest with d: {0}", s);
                IEnumerable<string> FileNames = from f in FileList where f.EndsWith(s) select f;
                foreach (string FileName in FileNames)
                {
                    lines = File.ReadAllLines(FileName);
                    foreach (string line in lines)
                        results.Add(line);
                }
            }

            return results;
        }
    }
}
