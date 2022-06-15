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
        public static List<string> encodebadchars(List<string> results)
        {
            List<string> tmp = new List<string>();
            foreach (string r in results)
            {
                tmp.Add(Uri.EscapeDataString(r));
                tmp.Add(HttpUtility.UrlEncode(r));
                tmp.Add(HttpUtility.UrlEncodeUnicode(r));
                tmp.Add(HttpUtility.HtmlEncode(r));
                tmp.Add(Convert.ToBase64String(Encoding.UTF8.GetBytes(r)));

                //Escape quotes
                tmp.Add(EscapeQuotes(r));

                //hexEscape
                tmp.Add(HexEscapeString(r));

                //html ascii:
                tmp.Add(AsciiEncode(r));
                tmp.Add(AsciiHexEncode(r));
            }

            results.AddRange(tmp);
            return results.Distinct().ToList();

        }



        public static string EscapeQuotes(string str)
        {
            return str.Replace("\\", "\\\\").Replace("\"", "\\\"").Replace("'", "\\'");
        }


        public static string HexEscapeString(string str)
        {
            string hexstr = string.Empty;
            foreach (char c in str)
            {
                try { hexstr += Uri.HexEscape(c); }
                catch { hexstr += c; }               
            }
            return hexstr;
        }

        public static string AsciiEncode(string str)
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


        public static string AsciiHexEncode(string str)
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
