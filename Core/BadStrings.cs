using System;
using System.Linq;
using System.Web;
using System.Collections.Generic;


namespace CShidori.Core
{
    public class BadStrings
    {
        public List<string> Output { get; set; }

        public BadStrings()
        {

            List<string> results = new List<string>();

            string[] lines = System.IO.File.ReadAllLines(@"Data/BadStrings.txt");
            foreach (string line in lines)
            {
                results.Add(line);
            }

            String[] bof = new string[] {
                string.Concat(Enumerable.Repeat("%s", 10)), // format string
                string.Concat(Enumerable.Repeat("%n", 10)) , // format string
                string.Concat(Enumerable.Repeat("A", 500)) //BoF
            };
            results.AddRange(bof);

            results = encodebadchars(results); //results.Distinct().ToList();

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

                //Escape quotes
                r2 = r.Replace("\"", "\\\"").Replace("'","\\'");
                if (r != r2)
                {
                    tmp.Add(r2);
                }
                
                //hexEscape
                hexstr = string.Empty;
                foreach(char c in r)
                {
                    try
                    {
                        hexstr += Uri.HexEscape(c);
                    }
                    catch
                    {
                        hexstr += c;
                    }
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
                    catch
                    {
                        htmlASCIIEndoed += c;
                    }
                }
                tmp.Add(htmlASCIIEndoed);
                tmp.Add(htmlASCIIHexEndoed);
            }
       
            results.AddRange(tmp);
            return results.Distinct().ToList();

        }
    
    }

}
