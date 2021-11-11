using System;
using System.Collections.Generic;
using System.Linq;

namespace CShidori
{
    class Program
    {
        static void Main(string[] args)
        {
            string executable = System.AppDomain.CurrentDomain.FriendlyName;

            if (args.Length >= 3 && args[0].ToLower() == "mut")
            {
                mutation mut = new mutation(int.Parse(args[1]), args[2] );
                Console.WriteLine(String.Join("\n", mut.Output));
            }
            else if (args.Length >= 1 && args[0].ToLower() == "bc")
            {
               badChars bc = new badChars();
               Console.WriteLine(String.Join("\n", bc.Output));
            }
            else if (args.Length >= 2 && args[0].ToLower() == "xss")
            {
                new xssInjection(args[1]);
            }
            else if(args.Length >= 3 && args[0].ToLower() == "json")
            {
                new jsonInjection(args[1], args[2]);
            }
            else if(args.Length >= 2 && args[0].ToLower() == "soap")
            {
                new soapInjection(args[1]);
            }
            else if (args.Length >= 3 && args[0].ToLower() == "xml")
            {
                new xmlInjection(args[1], args[2]);
            }
            else if (args.Length >= 2 && args[0].ToLower() == "xsd")
            {
                new xsdInjection(args[1]);
            }
            else if (args.Length >= 3 && args[0].ToLower() == "get")
            {
                 new getInjection(args[1], args[2]);
            }
            else if (args.Length >= 4 && args[0].ToLower() == "csrf")
            {
                 new csrfTemplate(args[1], args[2], args[3]);
            }
            else if (args.Length >= 3 && args[0].ToLower() == "xxe")
            {
                new xxeTemplate(args[1], args[2]);
            }
            else if (args.Length >= 2 && args[0].ToLower() == "enc")
            {
                List<string> l = new List<string>() { args[1] };
                Console.WriteLine(String.Join("\n", new badChars().encodebadchars(l)));
            }
            else if (args.Length >= 1 && args[0].ToLower() == "help")
            {
                //help
                Console.WriteLine("# CShidori: payload generator helper\tversion: 1.1.8");
                Console.WriteLine("\tMutation: {0} mut <count> <string>", executable);
                Console.WriteLine("\tBadChars: {0} bc", executable);
                Console.WriteLine("\tXSS/Injection: {0} xss <input> ", executable);
                Console.WriteLine("\tJSON: {0} json <json parameters> <input>", executable);
                Console.WriteLine("\tXML: {0} xml <file.xml>", executable);
                Console.WriteLine("\tSOAP: {0} soap <file.wsdl>", executable);
                Console.WriteLine("\tXSD: {0} xsd <file.xsd>", executable);
                Console.WriteLine("\tGET: {0} get <[url]?parameters>", executable);
                Console.WriteLine("\tCSRF: {0} csrf <http-method> <url>", executable);
                Console.WriteLine("\tXXE: {0} xxe <http-listener-ip>", executable);
                Console.WriteLine("\tEncode: {0} enc <string>", executable);
            }

        }
    }
}
