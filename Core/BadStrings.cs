using System;
using System.Linq;
using System.Web;
using System.Text;
using System.Collections.Generic;


namespace CShidori.Core
{
    public class BadStrings
    {
        public List<string> Output { get; set; }

        public BadStrings()
        {
            string[] lines;
            List<string> results = new List<string>();

            lines = System.IO.File.ReadAllLines(@"Data/BadChars.txt");
            foreach (string line in lines)
                results.Add(line);

            lines = System.IO.File.ReadAllLines(@"Data/BadStrings.txt");
            foreach (string line in lines)
                results.Add(line);

            lines = System.IO.File.ReadAllLines(@"Data/Java.txt");
            foreach (string line in lines)
                results.Add(line);

            lines = System.IO.File.ReadAllLines(@"Data/DotNet.txt");
            foreach (string line in lines)
                results.Add(line);

            results = encodebadchars(results); //results.Distinct().ToList();

            String[] bof = new string[] {
                string.Concat(Enumerable.Repeat("%s", 10)), // format string
                string.Concat(Enumerable.Repeat("%n", 10)) , // format string
                string.Concat(Enumerable.Repeat("A", 500)) //BoF
            };
            results.AddRange(bof);

            this.Output = results;
        }


        public List<string> encodebadchars ( List<string> results)
        {
            string hexstr;
            string r2;
            string htmlASCIIEndoed;
            string htmlASCIIHexEndoed;
            int val;
            List<string> tmp = new List<string>();
            foreach (string r in results)
            {
                tmp.Add(Uri.EscapeDataString(r));
                tmp.Add(HttpUtility.UrlEncode(r));
                tmp.Add(HttpUtility.UrlEncodeUnicode(r));
                tmp.Add(HttpUtility.HtmlEncode(r));
                tmp.Add(Convert.ToBase64String(Encoding.UTF8.GetBytes(r)));

                //Escape quotes
                r2 = r.Replace("\\","\\\\").Replace("\"", "\\\"").Replace("'","\\'");
                if (r != r2)
                {
                    tmp.Add(r2);
                }
                
                //hexEscape
                hexstr = string.Empty;
                foreach(char c in r)
                {
                    try{ hexstr += Uri.HexEscape(c); }
                    catch{ hexstr += c; }
                }
                tmp.Add(hexstr);


                //html ascii:
                htmlASCIIEndoed = string.Empty;
                htmlASCIIHexEndoed = string.Empty;
                foreach (char c in r)
                {
                    try
                    {
                        val = Convert.ToInt32(c);
                        if(val < 128) //if ASCCI not extended
                        {
                            htmlASCIIEndoed += "&#" + val.ToString() + ";";
                            htmlASCIIHexEndoed += "&#x" + val.ToString("X") + ";";
                        }
                    }
                    catch{ htmlASCIIEndoed += c; }
                }

                tmp.Add(htmlASCIIEndoed);
                tmp.Add(htmlASCIIHexEndoed);
            }
       
            results.AddRange(tmp);
            return results.Distinct().ToList();

        }
    
    }

}
