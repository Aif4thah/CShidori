using Microsoft.VisualStudio.TestTools.UnitTesting;
using CShidori.DataHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CShidori.Core;
using CShidori.Generator;

namespace CShidori.DataHandler.Tests
{
    [TestClass()]
    public class DataLoaderTests
    {
        [TestMethod()]
        public void DataLoaderTest()
        {
            new DataLoader("Chars, BadString, DotNet");
            Assert.IsTrue(BadStrings.Output.Count > 0);
        }
    }
}