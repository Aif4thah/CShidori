using System;
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
                description: "mode: function you want to call (mut, gen, enc, ...)");

            var o = new Option<string>(
                "-o",
                getDefaultValue: () => "1",
                description: "option: option needed by the called function ");

            var i = new Option<string>( 
                "-i",
                getDefaultValue: () => "foo",
                description: "input: data you want processing");

            var p = new Option<string>( 
                "-p",
                getDefaultValue: () => "bar",
                description: "parameter: additional parameters");

            var d = new Option<string>( 
                "-d",
                getDefaultValue: () => "s",
                description: "data: used for generation techniques (Chars,String,Java,Dotnet,...)");

            var rootCommand = new RootCommand{m,i,p, o,d};

            rootCommand.Description = @"
CShidori : 1024 Birds to your fuzzer
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
                        new MutDispatcher(int.Parse(o), i).Output.ForEach(x => Console.WriteLine(x));
                        break;

                    case "enc":
                        List<string> list = new List<string>() { i };
                        EncodeStrings.encodebadchars(list).ForEach(x => Console.WriteLine(x));
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
