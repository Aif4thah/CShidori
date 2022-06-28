using Microsoft.VisualStudio.TestTools.UnitTesting;
using CShidori.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CShidori.DataHandler;
using CShidori.Generator;

namespace CShidori.Core.Tests
{
    [TestClass()]
    public class EncodeStringsTests
    {
        [TestMethod()]
        public void EncodeStringTest()
        {
            string p = Misc.RandomString(10);
            List<string> list = new List<string>() { p };
            List<string> results = EncodeStrings.encodebadchars(list);
            Assert.IsTrue(results.Count > list.Count);

        }

        public void EncodeStringsFuzz()
        {
            string p = Misc.RandomString(10);
            new DataLoader("Chars, BadString, DotNet");
            List<string> results = EncodeStrings.encodebadchars(BadStrings.Output);
            Assert.IsTrue(results.Count > BadStrings.Output.Count);

        }
    }
}