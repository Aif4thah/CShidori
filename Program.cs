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

            var m = new Option<string>( //mode
                "-m",
                getDefaultValue: () => "none",
                description: "mode, ie: bc, xss, json...");

            var o = new Option<string>( // option
                "-o",
                getDefaultValue: () => "none",
                description: "option, ie: get, post...");

            var i = new Option<string>( // injection
                "-i",
                getDefaultValue: () => "foo",
                description: "injection, ie: foo");

            var p = new Option<string>( //params
                "-p",
                getDefaultValue: () => "bar",
                description: "parameter, ie: bar");

            var d = new Option<string>( //data
            "-d",
            getDefaultValue: () => "s",
            description: "parameter, ie: bar");

            var rootCommand = new RootCommand{m,i,p, o,d};

            rootCommand.Description = @"
CShidori : A C# Thousand Birds Payloads Generator
README: https://github.com/Aif4thah/CShidori
License: GPL-3.0
Disclaimer: Usage of this tool for attacking targets without prior mutual consent is illegal. It is the end user's responsibility to obey all applicable local, state and federal laws. We assume no liability and are not responsible for any misuse or damage caused by this site. This tools is provided 'as is' without warranty of any kind.";
            
            rootCommand.SetHandler((string m, string i, string p, string o, string d) =>
            {

                switch (m)
                {
                    case "tls":
                        NetworkTest.TlsFuzz.TlsFuzzAsync(o, i, p, d);
                        break;

                    case "tcp":
                        NetworkTest.TcpFuzz.TcpFuzzAsync(o, i, p, d);
                        break;

                    case "gen":
                        BadStrings data = new BadStrings(d);
                        Console.WriteLine(String.Join("\n", data.Output));
                        break;

                    case "mut":
                        Mutation mut = new Mutation(int.Parse(o), p, d);
                        Console.WriteLine(String.Join("\n", mut.Output));
                        break;

                    case "enc":
                        List<string> list = new List<string>() { p };
                        List<string> results = EncodeStrings.encodebadchars(list);
                        Console.WriteLine(String.Join("\n", results));
                        break;

                    case "json":
                        BadStrings DataToJson = new BadStrings(d);
                        foreach (string bs in DataToJson.Output)
                            new JsonInjection(p, bs);                     
                        break;

                    case "xml":
                        BadStrings DataToXml = new BadStrings(d);
                        foreach (string bs in DataToXml.Output)
                            new XmlInjection(p, bs);
                        break;

                    case "get":
                        BadStrings DataToGet = new BadStrings(d);
                        foreach (string bs in DataToGet.Output)
                            new GetInjection(p, bs);
                        break;

                    case "csrf":
                        new CsrfTemplate(o, p, i);
                        break;

                    case "xxe":
                        new XxeTemplate(o);
                        break;

                    case "mst":
                        new UnitTest.MsTestTemplate(p, i);
                        break;

                    case "xss":
                        new XssInjection(p);
                        break;

                }

            }, m,i,p,o,d);

            rootCommand.Invoke(args);

        }
    }
}
