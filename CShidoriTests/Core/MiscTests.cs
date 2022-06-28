using Microsoft.VisualStudio.TestTools.UnitTesting;
using CShidori.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace CShidori.Core.Tests
{
    [TestClass()]
    public class MiscTests
    {
        [TestMethod()]
        public void MiscTest()
        {
            string r = Misc.RandomString(10);
            Assert.IsTrue(r.Length == 10);

            IPAddress ipVar;
            string ip = Misc.GetIp();
            Assert.IsTrue(IPAddress.TryParse(ip, out ipVar));
        }
    }
}