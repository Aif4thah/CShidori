using Microsoft.VisualStudio.TestTools.UnitTesting;
using CShidori;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.CommandLine;
using CShidori.Core;
using CShidori.DataHandler;
using CShidori.NetworkTest;

namespace CShidori.Tests
{
    [TestClass()]
    public class ProgramTests
    {
        [TestMethod()]
        public void RandomStringTest()
        {
            string r = Misc.RandomString(10);
            Assert.IsTrue( r.Length == 10);
        }


        [TestMethod()]
        public void BadStringTest()
        {
            new DataLoader("Chars, BadString, DotNet");
            Assert.IsTrue(BadStrings.Output.Count > 0);
        }

        [TestMethod()]
        public void MutationTest()
        {
            int o = 10;
            string p = Misc.RandomString(10);
            new DataLoader("Chars, BadString, DotNet");
            Mutation result = new Mutation(o, p);
            Assert.IsTrue(result.Output.Count == o);

        }

        [TestMethod()]
        public void EncodeStringTest()
        {
            string p = Misc.RandomString(10);
            List<string> list = new List<string>() { p };
            List<string> results = EncodeStrings.encodebadchars(list);
            Assert.IsTrue(results.Count > list.Count);

        }
    }
}
