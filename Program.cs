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
                Mutation mut = new Mutation(int.Parse(args[1]), args[2] );
                Console.WriteLine(String.Join("\n", mut.Output));
            }
            else if (args.Length >= 1 && args[0].ToLower() == "bc")
            {
               BadChars bc = new BadChars();
               Console.WriteLine(String.Join("\n", bc.Output));
            }
            else if (args.Length >= 2 && args[0].ToLower() == "xss")
            {
                new XssInjection(args[1]);
            }
            else if(args.Length >= 3 && args[0].ToLower() == "json")
            {
                new JsonInjection(args[1], args[2]);
            }
            else if(args.Length >= 2 && args[0].ToLower() == "soap")
            {
                new SoapInjection(args[1]);
            }
            else if (args.Length >= 3 && args[0].ToLower() == "xml")
            {
                new XmlInjection(args[1], args[2]);
            }
            else if (args.Length >= 2 && args[0].ToLower() == "xsd")
            {
                new XsdInjection(args[1]);
            }
            else if (args.Length >= 3 && args[0].ToLower() == "get")
            {
                 new GetInjection(args[1], args[2]);
            }
            else if (args.Length >= 4 && args[0].ToLower() == "csrf")
            {
                 new CsrfTemplate(args[1], args[2], args[3]);
            }
            else if (args.Length >= 2 && args[0].ToLower() == "xxe")
            {
                new XxeTemplate(args[1]);
            }
            else if (args.Length >= 2 && args[0].ToLower() == "enc")
            {
                List<string> l = new List<string>() { args[1] };
                Console.WriteLine(String.Join("\n", new BadChars().encodebadchars(l)));
            }
            else if (args.Length >= 1 && args[0].ToLower() == "help")
            {
                //help
                Console.WriteLine("# CShidori: payload generator helper\tversion: 1.1.9");
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
