using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CShidori.DataHandler;
using CShidori.Core;

namespace CShidori.UnitTest
{
    public class MsTestTemplate
    {
        public string function { get; set; }
        public string inject { get; set; }
        public string Output { get; set; }

        public MsTestTemplate (string functionWithParam, string paramToInject)
        {
            this.function = functionWithParam; //ie: "function(string test1, string test2)"
            this.inject = paramToInject;
            foreach(string l in BadStrings.Output)
                Console.WriteLine(l.Replace("%", this.inject).Replace("§",this.function));

        }
    }
}
