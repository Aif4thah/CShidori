using Microsoft.VisualStudio.TestTools.UnitTesting;
using CShidori;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.CommandLine;
using System.Net;
using System.Net.Sockets;
using CShidori.Core;
using CShidori.DataHandler;
using CShidori.NetworkTest;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace CShidori.Tests
{
    [TestClass()]
    public class ProgramTests
    {

       /*
        *   Core
        */
        [TestMethod()]
        public void MiscTest()
        {
            string r = Misc.RandomString(10);
            Assert.IsTrue( r.Length == 10);

            IPAddress ipVar;
            string ip = Misc.GetIp();
            Assert.IsTrue(IPAddress.TryParse(ip, out ipVar));
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

        /*
        *   [ Network ]
        */

        [TestMethod()]
        public void TlsFuzzTest()
        {
            int testDuration = 1024 * 2;
            var certificate = new X509Certificate2(@"UnitTestData/DotNotUseInProd.pfx", "cshidori");           
            var listener = new TcpListener(IPAddress.Loopback, 10443);
            

            // fuzzer send
            new DataLoader("Chars");
            Task.Run(() =>
            {
                TlsFuzz.TlsFuzzAsync(@"UnitTestData/request.txt", "127.0.0.1", "10443");
            });
            listener.Start();
            int i;
            for(i=0; i < testDuration; i++)
            {
                // serveur listen
                var client = listener.AcceptTcpClient();
                var stream = client.GetStream();
                SslStream sslStream = new SslStream(stream, false);
                sslStream.AuthenticateAsServer(certificate, false, System.Security.Authentication.SslProtocols.Tls12, false);

                // serveur read and respond
                while(client.Connected)
                {
                    byte[] buffer = new byte[client.ReceiveBufferSize];
                    int bytesRead = sslStream.Read(buffer, 0, client.ReceiveBufferSize);
                    string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    Console.WriteLine("Server reciv: {0}", message);
                    sslStream.Write(Encoding.UTF8.GetBytes(message), 0, Encoding.UTF8.GetBytes(message).Length);
                    client.Close();
                }

            }
            Console.WriteLine(i);
            Assert.IsTrue(i == testDuration);
        }



        [TestMethod()]
        public void TcpFuzzTest()
        {
            int testDuration = 1024 * 2;
            var listener = new TcpListener(IPAddress.Loopback, 1080);

            // fuzzer send
            new DataLoader("Chars");
            Task.Run(() =>
            {
                TcpFuzz.TcpFuzzAsync(@"UnitTestData/request.txt", "127.0.0.1", "1080");
            });
            listener.Start();
            int i;
            for (i = 0; i < testDuration ; i++)
            {
                // serveur listen
                var client = listener.AcceptTcpClient();

                // serveur read and respond
                while (client.Connected)
                {
                    var stream = client.GetStream();
                    stream.ReadTimeout = 2000;
                    byte[] buffer = new byte[4096];
                    int bytesRead = stream.Read(buffer, 0, buffer.Length);
                    string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    Console.WriteLine("Server reciv: {0}", message);
                    stream.Write(Encoding.UTF8.GetBytes(message), 0, Encoding.UTF8.GetBytes(message).Length);
                    client.Close();
                }

            }
            Console.WriteLine(i);
            Assert.IsTrue(i == testDuration);
        }


        /*
         *   [ Fuzzing and introspection Tests ]
         */


        [TestMethod()]
        public void MutationFuzz()
        {
            int o = 1024;
            string p = Misc.RandomString(10);
            new DataLoader("Chars, BadString, DotNet");
            Mutation result = new Mutation(o, p);
            result.Output.ForEach(x => new Mutation(o, x));

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
