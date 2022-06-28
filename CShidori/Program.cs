﻿using System;
using System.Diagnostics;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.CommandLine;
using CShidori.Core;
using CShidori.DataHandler;
using CShidori.Generator;

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
CShidori : A C# Thousand Birds Payloads Fuzzer
License: GPL-3.0
Disclaimer: Usage of this tool for attacking targets without prior mutual consent is illegal. It is the end user's responsibility to obey all applicable local, state and federal laws. We assume no liability and are not responsible for any misuse or damage caused by this site. This tools is provided 'as is' without warranty of any kind.
DOCUMENTATION: https://github.com/Aif4thah/CShidori
";
            
            rootCommand.SetHandler((string m, string i, string p, string o, string d) =>
            {

                switch (m)
                {

                    //Core
                    case "gen":
                        new DataLoader(d);
                        BadStrings.Output.ForEach(x => Console.WriteLine(x));
                        break;
                                        
                    case "mut":
                        new DataLoader(d);
                        MutDispatcher mut = new MutDispatcher(int.Parse(o), p);
                        mut.Output.ForEach(x => Console.WriteLine(x));
                        break;

                    case "enc":
                        List<string> list = new List<string>() { p };
                        List<string> results = EncodeStrings.encodebadchars(list);
                        results.ForEach(x => Console.WriteLine(x));
                        break;
                    
                    //Generator
                    case "csrf":
                        new DataLoader("CsrfTemplate");
                        new CsrfTemplate(o, p, i);
                        break;

                    case "xxe":
                        new DataLoader("XxeTemplate");
                        new XxeTemplate(o);
                        break;


                }

            }, m,i,p,o,d);

            rootCommand.Invoke(args);
        }
    }
}
