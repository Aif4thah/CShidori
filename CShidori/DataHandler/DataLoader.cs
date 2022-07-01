using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CShidori.Core;

namespace CShidori.DataHandler
{
    public class DataLoader
    {
        public DataLoader(string d)
        {
            List<string> results = new List<string>();
            List<string> FileList = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory + @"/Data").ToList();
            
            foreach ( string s in d.Split(","))
            {
                IEnumerable<string> FileNames = from f in FileList where f.EndsWith(s) select f;
                foreach (string FileName in FileNames)
                {
                    foreach (string line in File.ReadAllLines(FileName))
                        results.Add(line);
                }
            }
            BadStrings.Output = d.EndsWith("Template") ? results : EncodeStrings.encodebadchars(results); //EncodeStrings.encodebadchars contains results.Distinct().ToList();
        }
    }
}
