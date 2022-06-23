using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CShidori.DataHandler;

namespace CShidori.Core
{
    public class XxeTemplate
    {

        string target { get; set; }
        public XxeTemplate(string TemplateType)
        {
            this.target = Misc.GetIp();
            foreach(string l in BadStrings.Output)
                Console.WriteLine(l.Replace("§", this.target).Replace("foo", Misc.RandomString(3)));
        }
    }
}
