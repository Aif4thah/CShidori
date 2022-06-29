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
            Random rand = new Random();
            string p = Misc.RandomString(10);
            new DataLoader("Chars, BadString, DotNet");
            string result = Mutation.RepRandBc(rand, p);
            Assert.IsTrue(result.Length >= p.Length && result != p);
        }

        [TestMethod()]
        public void AddRandBcTest()
        {
            Random rand = new Random();
            string p = Misc.RandomString(10);
            new DataLoader("Chars, BadString, DotNet");
            string result = Mutation.AddRandBc(rand, p);
            Assert.IsTrue(result.Length > p.Length);
        }

        [TestMethod()]
        public void RepLineTest()
        {
            Random rand = new Random();
            string p = Misc.RandomString(10);
            new DataLoader("Chars, BadString, DotNet");
            string result = Mutation.RepLine(rand, p);
            Assert.IsTrue(result != String.Empty && result != p);
        }

        [TestMethod()]
        public void DelCharTest()
        {
            Random rand = new Random();
            string p = Misc.RandomString(10);
            new DataLoader("Chars, BadString, DotNet");
            string result = Mutation.DelChar(rand, p);
            Assert.IsTrue(result != String.Empty && result.Length < p.Length);
        }

        [TestMethod()]
        public void BitFlipTest()
        {
            Random rand = new Random();
            string p = Misc.RandomString(10);
            new DataLoader("Chars, BadString, DotNet");
            string result = Mutation.BitFlip(rand, p);
            Assert.IsTrue(result != p && result.Length == p.Length);
        }

        [TestMethod()]
        public void RepThreeBytesTest()
        {
            Random rand = new Random();
            string p = Misc.RandomString(10);
            new DataLoader("Chars, BadString, DotNet");
            string result = Mutation.RepThreeBytes(rand, p);
            Assert.IsTrue(result != p && result.Length == p.Length);
        }

        [TestMethod()]
        public void RepeatStrTest()
        {
            Random rand = new Random();
            string p = Misc.RandomString(2);
            new DataLoader("Chars, BadString, DotNet");
            string result = Mutation.RepeatStr(rand, p);
            Console.WriteLine("result: {0} and p = {1}", result, p);
            Assert.IsTrue(result.Length >= p.Length);
            
        }
    }

}