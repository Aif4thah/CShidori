using Microsoft.VisualStudio.TestTools.UnitTesting;
using CShidori.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CShidori.DataHandler;

namespace CShidori.Core.Tests
{
    [TestClass()]
    public class MutDispatcherTests
    {
        [TestMethod()]
        public void MutDispatcherTest()
        {

            int o = 10;
            string p = Misc.RandomString(10);
            new DataLoader("Chars, BadString, DotNet");
            MutDispatcher result = new MutDispatcher(o, p);
            Assert.IsTrue(result.Output.Count == o);

        }

        [TestMethod()]
        public void MutationFuzz()
        {
            int o = 4096;
            string p = Misc.RandomString(10);
            new DataLoader("Chars, BadString, DotNet");
            MutDispatcher result = new MutDispatcher(o, p);
            result.Output.ForEach(x => new MutDispatcher(o, x));

        }

    }
}