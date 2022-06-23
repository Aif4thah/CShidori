using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CShidori.Core;
using CShidori.DataHandler;

namespace CShidori.Generator
{
    public class XxeTemplate
    {

        string target { get; set; }
        public XxeTemplate(string TemplateType)
        {
            target = Misc.GetIp();
            foreach (string l in BadStrings.Output)
                Console.WriteLine(l.Replace("§", target).Replace("foo", Misc.RandomString(3)));
        }
    }
}
