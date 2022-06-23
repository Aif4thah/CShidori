using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Threading.Tasks;
using CShidori.DataHandler;

namespace CShidori.Core
{
    public static class EncodeStrings
    {
        public static List<string> encodebadchars(List<string> inputs)
        {
            List<string> results = new List<string>();
            results.AddRange(inputs);
            foreach (string input in inputs)
            {
                results.Add(Uri.EscapeDataString(input));
                results.Add(HttpUtility.UrlEncode(input));
                results.Add(HttpUtility.UrlEncodeUnicode(input));
                results.Add(HttpUtility.HtmlEncode(input));
                results.Add(Convert.ToBase64String(Encoding.UTF8.GetBytes(input)));

                //Escape quotes
                results.Add(EscapeQuotes(input));

                //hexEscape
                results.Add(HexEscapeString(input));

                //html ascii:
                results.Add(AsciiEncode(input));
                results.Add(AsciiHexEncode(input));
            }
            return results.Distinct().ToList();

        }



        private static string EscapeQuotes(string str)
        {
            return str.Replace("\\", "\\\\").Replace("\"", "\\\"").Replace("'", "\\'");
        }


        private static string HexEscapeString(string str)
        {
            string hexstr = string.Empty;
            foreach (char c in str)
            {
                try { hexstr += Uri.HexEscape(c); }
                catch { hexstr += c; }               
            }
            return hexstr;
        }

        private static string AsciiEncode(string str)
        {
            string htmlASCIIEncoded = string.Empty; ;
            foreach (char c in str)
            {
                try
                {
                    int val = Convert.ToInt32(c);
                    if (val < 128) //if ASCCI not extended
                    {
                        htmlASCIIEncoded += "&#" + val.ToString() + ";";
                    }
                }
                catch { htmlASCIIEncoded += c; }
            }
            return htmlASCIIEncoded;
        }


        private static string AsciiHexEncode(string str)
        {
            string htmlASCIIHexEncoded = string.Empty; ;
            foreach (char c in str)
            {
                try
                {
                    int val = Convert.ToInt32(c);
                    if (val < 128) //if ASCCI not extended
                    {
                        htmlASCIIHexEncoded += "&#x" + val.ToString("X") + ";";
                    }
                }
                catch { htmlASCIIHexEncoded += c; }
            }
            return htmlASCIIHexEncoded;


        }
    }
}
