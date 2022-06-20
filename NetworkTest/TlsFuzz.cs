using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Collections.ObjectModel;
using System.Threading;
using System.Text.RegularExpressions;
using System.Diagnostics;
using CShidori.DataHandler;

namespace CShidori.NetworkTest
{
    public class TlsFuzz
    {
        public static async void TlsFuzzAsync(string File, string Ip, string Port, string data)
        {
            bool FirstReq = true;
            string LogFile = Ip + "-" + Guid.NewGuid().ToString();


            Console.WriteLine("[*] Reading File: {0}", File);
            string req = System.IO.File.ReadAllText(File);

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            double TotalReq = req.Length * 200;
            double ElapsedTime;

            Console.WriteLine("[*] Send Original request");
            await SslOneReq(req, Ip, Port, data, LogFile);

            Console.WriteLine("[*] Start Fuzzing for {0} requests", TotalReq);
            int i = 1;
            while (i <= TotalReq)
            {
                
                ElapsedTime = (int)((stopwatch.ElapsedMilliseconds) + 1) / 1000;
                Console.WriteLine("[{0:0.00} %]\t ETA:{1}\t Speed: {2:0.00}/s \t {3} requests",
                    ((i / TotalReq) * 100), // % of work
                    DateTime.Now.AddSeconds((ElapsedTime / i) * (TotalReq - i)), //ETA (Nom + Remain time
                    i / ElapsedTime, //Speed
                    i
                );

                try
                {
                    Core.Mutation mut = new Core.Mutation(300, req, data);
                    foreach (string str in mut.Output)
                    {
                        await SslOneReq(str, Ip, Port, data, LogFile);
                        i++;
                        //Thread.Sleep(300); /* use this to avoid Firewall Ban or DOS
                    }

                }
                catch (Exception ex)
                {
                    Thread.Sleep(100);
                    new DataLogWriter(LogFile, Guid.NewGuid(), "Internal Error", ex.ToString());
                    Console.Write(ex.ToString());
                }

            }
                
        }


        private static async Task SslOneReq(string req, string Ip, string Port, string data, string LogFile)
            {
            Guid uuid = Guid.NewGuid();
            string rsp = String.Empty;
            TcpClient client = new TcpClient(Ip, int.Parse(Port));
            var stream = client.GetStream();
            stream.ReadTimeout = 2000;
            SslStream sslStream = new SslStream(stream, false, new RemoteCertificateValidationCallback(CertificateValidationCallback));
            sslStream.AuthenticateAsClient("client", null, System.Security.Authentication.SslProtocols.Tls12, false);

            sendMsg(req, sslStream);
            
            for(int n = 0; n < 5; n++){ rsp += readMsg(sslStream, client);}

            new DataLogWriter(LogFile, uuid, req, rsp);

            sslStream.Close();
            client.Close();

        }


        private static bool CertificateValidationCallback(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        private static string readMsg(SslStream sslStream, TcpClient client)
        {
            //Console.WriteLine("[reciv]");
            byte[] buffer = new byte[client.ReceiveBufferSize];
            int bytesRead = sslStream.Read(buffer, 0, client.ReceiveBufferSize);
            return Encoding.UTF8.GetString(buffer, 0, bytesRead);
        }
        private static void sendMsg(string message, SslStream sslStream)
        {
            //Console.WriteLine("[send]");
            sslStream.Write(Encoding.UTF8.GetBytes(message), 0, Encoding.UTF8.GetBytes(message).Length);
        }
    }
}
