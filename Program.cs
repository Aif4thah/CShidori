using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.CommandLine;
using CShidori.Core;

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

            rootCommand.Description = @"CShidori : A C# Thousand Birds Payloads Generator
README: https://github.com/Aif4thah/CShidori
Licence: GPL-3.0
Disclaimer: Usage of this tool for attacking targets without prior mutual consent is illegal. It is the end user's responsibility to obey all applicable local, state and federal laws. We assume no liability and are not responsible for any misuse or damage caused by this site. This tools is provided 'as is' without warranty of any kind.";
            
            rootCommand.SetHandler((string m, string i, string p, string o) =>
            {
                BadStrings bss = new BadStrings();

                switch (m)
                {
                    case "dump":
                        Console.WriteLine(String.Join("\n", bss.Output));
                        break;

                    case "mut":
                        Mutation mut = new Mutation(int.Parse(o), p);
                        Console.WriteLine(String.Join("\n", mut.Output));
                        break;

                    case "enc":
                        List<string> l = new List<string>() { p };
                        Console.WriteLine(String.Join("\n", bss.encodebadchars(l)));
                        break;

                    case "xss":
                        new XssInjection(p);
                        break;

                    case "json":
                        foreach (string bs in bss.Output)
                            new JsonInjection(p, bs);                     
                        break;

                    case "xml":
                        foreach (string bs in bss.Output)
                            new XmlInjection(p, bs);
                        break;

                    case "get":
                        foreach (string bs in bss.Output)
                            new GetInjection(p, bs);
                        break;

                    case "csrf":
                        new CsrfTemplate(o, p, i);
                        break;

                    case "xxe":
                        new XxeTemplate(o);
                        break;

                    case "mst":
                        new MsTestTemplate(p, i);
                        break;

                }

            }, m,i,p,o);

            rootCommand.Invoke(args);


        }
    }
}
