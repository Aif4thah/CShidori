using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CShidori.Core
{
    public class XxeTemplate
    {

        string target { get; set; }
        public XxeTemplate(string TemplateType)
        {
            this.target = Misc.GetIp();
            string XXE = System.IO.File.ReadAllText(@"Data/XxeTemplate.txt");
            Console.WriteLine(
                XXE.Replace("§", this.target).Replace("foo", Misc.RandomString(3)));

        }
    }
}
