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
    public class MutationTests
    {

        
        [TestMethod()]
        public void RepRandBcTest()
        {
            string p = Misc.RandomString(10);
            new DataLoader("Chars, BadString, DotNet");
            string result = Mutation.RepRandBc(p);
            Assert.IsTrue(result.Length >= p.Length && result != p);
        }

        [TestMethod()]
        public void AddRandBcTest()
        {
            string p = Misc.RandomString(10);
            new DataLoader("Chars, BadString, DotNet");
            string result = Mutation.AddRandBc(p);
            Assert.IsTrue(result.Length > p.Length);
        }

        [TestMethod()]
        public void RepLineTest()
        {
            string p = Misc.RandomString(10);
            new DataLoader("Chars, BadString, DotNet");
            string result = Mutation.RepLine(p);
            Assert.IsTrue(result != String.Empty && result != p);
        }

        [TestMethod()]
        public void DelCharTest()
        {
            string p = Misc.RandomString(10);
            new DataLoader("Chars, BadString, DotNet");
            string result = Mutation.DelChar(p);
            Assert.IsTrue(result != String.Empty && result.Length < p.Length);
        }

        [TestMethod()]
        public void BitFlipTest()
        {
            string p = Misc.RandomString(10);
            new DataLoader("Chars, BadString, DotNet");
            string result = Mutation.BitFlip(p);
            Assert.IsTrue(result != p && result.Length == p.Length);
        }

        [TestMethod()]
        public void RepThreeBytesTest()
        {
            string p = Misc.RandomString(10);
            new DataLoader("Chars, BadString, DotNet");
            string result = Mutation.RepThreeBytes(p);
            Assert.IsTrue(result != p && result.Length == p.Length);
        }

        [TestMethod()]
        public void RepeatStrTest()
        {
            string p = Misc.RandomString(10);
            new DataLoader("Chars, BadString, DotNet");
            string result = Mutation.RepeatStr(p);
            Console.WriteLine("result: {0} and p = {1}", result, p);
            Assert.IsTrue(result.Length >= p.Length);
            
        }
    }

}