using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CShidori.Core
{
    public class MsTestTemplate
    {
        public string function { get; set; }
        public string inject { get; set; }
        public string Output { get; set; }

        public MsTestTemplate (string functionWithParam, string paramToInject)
        {
            this.function = functionWithParam; //function(string test1, string test2)
            this.inject = paramToInject;
            string MsTest = System.IO.File.ReadAllText(@"Data/MsTestTemplate.txt");

            MsTest = MsTest.Replace("%", this.inject).Replace("§",this.function);

            Console.WriteLine(MsTest);


        }
    }
}
