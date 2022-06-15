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
            DateTime ETA = DateTime.Now;
            double TotalReq = req.Length * 10;
            double RemainTime = 0;
            double PourcentWork, ElapsedTime;
                      
            Console.WriteLine("[*] Start Fuzzing for {0} requests", TotalReq);
            for (int i = 0; i < TotalReq; i++)
            {
                PourcentWork = ((i / TotalReq) * 100);
                ElapsedTime = (int)(stopwatch.ElapsedMilliseconds)/1000;

                if(ElapsedTime > 0)
                {
                    RemainTime = (ElapsedTime / i) * (TotalReq - i);
                    ETA = DateTime.Now.AddSeconds(RemainTime);
                    Console.WriteLine("[{0} %]\t ETA:{1}", PourcentWork, ETA);
                }
                

                try
                {
                    await SslOneReq(req, Ip, Port, data, LogFile, FirstReq);
                    if (FirstReq) { FirstReq = false; }
                    Thread.Sleep(300);
                }
                catch(Exception ex)
                {
                    Thread.Sleep(1000);
                    new DataLogWriter(LogFile, Guid.NewGuid(), "Internal Error", ex.ToString());
                    Console.Write(ex.ToString());
                }

            }
                
        }


        public static async Task SslOneReq(string req, string Ip, string Port, string data, string LogFile, bool FirstReq)
            {
            Guid uuid = Guid.NewGuid();
            Core.Mutation mut = new Core.Mutation(1, req, data);

            TcpClient client = new TcpClient(Ip, int.Parse(Port));
            var stream = client.GetStream();
            SslStream sslStream = new SslStream(stream, false, new RemoteCertificateValidationCallback(CertificateValidationCallback));
            sslStream.AuthenticateAsClient("client", null, System.Security.Authentication.SslProtocols.Tls12, false);

            string rsp = string.Empty;
            foreach (string str in mut.Output) // Convert string[] mut.Output to string str
            {
                if (FirstReq){ sendMsg(req, sslStream); }
                else{          sendMsg(str, sslStream); }

                rsp = readMsg(sslStream, client);
                int n = 0;
                while (rsp == string.Empty)
                {
                    rsp = readMsg(sslStream, client);
                    if (n >= 1024) { break; }
                    n += 1;
                }
                new DataLogWriter(LogFile, uuid, str, rsp);

            }
            sslStream.Close();
            client.Close();

        }


        public static bool CertificateValidationCallback(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        public static string readMsg(SslStream sslStream, TcpClient client)
        {
            byte[] buffer = new byte[client.ReceiveBufferSize];
            int bytesRead = sslStream.Read(buffer, 0, client.ReceiveBufferSize);
            return Encoding.UTF8.GetString(buffer, 0, bytesRead);
        }
        public static void sendMsg(string message, SslStream sslStream)
        {
            sslStream.Write(Encoding.UTF8.GetBytes(message), 0, Encoding.UTF8.GetBytes(message).Length);
        }
    }
}
