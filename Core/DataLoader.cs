using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CShidori.Core
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
                "Data/Xss",
                "Data/CsrfTemplate",
                "Data/MsTestTemplate",
                "Data/XxeTemplate",

            };


            foreach( string s in d.Split(","))
            {
                //Console.WriteLine("DataLoader: Linq resquest with d: {0}", s);
                IEnumerable<string> FileNames = from f in FileList where f.EndsWith(s) select f;
                foreach (string FileName in FileNames)
                {
                    lines = System.IO.File.ReadAllLines(FileName);
                    foreach (string line in lines)
                        results.Add(line);
                }
            }




            return results;
        }
    }
}
