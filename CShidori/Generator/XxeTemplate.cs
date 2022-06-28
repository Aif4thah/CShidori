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
        public XxeTemplate(string TemplateType)
        {
            foreach (string l in BadStrings.Output)
                Console.WriteLine(l.Replace("§", Misc.GetIp()).Replace("foo", Misc.RandomString(3)));
        }
    }
}
