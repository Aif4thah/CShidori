using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.CommandLine;

namespace CShidori
{
    class Program
    {
        static void Main(string[] args)
        {

            var m = new Option<string>(
                "-m",
                getDefaultValue: () => "none",
                description: "mode, ie: bc, xss, json...");

            var o = new Option<string>(
                "-o",
                getDefaultValue: () => "none",
                description: "option, ie: get, post...");

            var i = new Option<string>(
                "-i",
                getDefaultValue: () => "foo",
                description: "injection, ie: foo");

            var p = new Option<string>(
                "-p",
                getDefaultValue: () => "bar",
                description: "parameter, ie: bar");

            var rootCommand = new RootCommand{m,i,p, o};

            rootCommand.Description = "CShidori : A C# Thousand Birds Payloads Generator";

            rootCommand.SetHandler((string m, string i, string p, string o) =>
            {
                switch(m)
                {
                    case "bc":
                        BadChars bc = new BadChars();
                        Console.WriteLine(String.Join("\n", bc.Output));
                        break;

                    case "mut":
                        Mutation mut = new Mutation(int.Parse(p), i);
                        Console.WriteLine(String.Join("\n", mut.Output));
                        break;

                    case "enc":
                        List<string> l = new List<string>() { p };
                        Console.WriteLine(String.Join("\n", new BadChars().encodebadchars(l)));
                        break;

                    case "xss":
                        new XssInjection(i);
                        break;

                    case "json":
                        new JsonInjection(p, i);
                        break;

                    case "xml":
                        new XmlInjection(p, i);
                        break;

                    case "get":
                        new GetInjection(p, i);
                        break;

                    case "csrf":
                        new CsrfTemplate(o, p, i);
                        break;

                    case "xxe":
                        new XxeTemplate(o);
                        break;

                }

            }, m,i,p,o);

            rootCommand.Invoke(args);


        }
    }
}
