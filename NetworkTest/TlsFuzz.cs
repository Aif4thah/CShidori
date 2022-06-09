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

namespace CShidori.NetworkTest
{
    public class TlsFuzz
    {
        public static async void TlsFuzzAsync(string File, string Ip, string Port, string data)
        {
            Console.WriteLine("[*] Reading File: {0}", File);
            string req = System.IO.File.ReadAllText(File);
            double TotalReq = req.Length * 10;
            Stopwatch stopwatch = new Stopwatch();
            double PourcentWork;
            double ElapsedTime;
            double RemainTime = 0;
            DateTime ETA = DateTime.Now;

            Console.WriteLine("[*] Start Fuzzing for {0} requests", TotalReq);
            stopwatch.Start();
            for (int i = 0; i < TotalReq; i++)
            {
                PourcentWork = ((i / TotalReq) * 100);
                ElapsedTime = (int)(stopwatch.ElapsedMilliseconds)/1000;

                if(ElapsedTime > 0)
                {
                    RemainTime = (ElapsedTime / i) * (TotalReq - i); //bug ??
                    ETA = DateTime.Now.AddSeconds(RemainTime);
                }

                Console.WriteLine("[{0} %]\tET: {1}\t ETA:{2}",
                    PourcentWork,
                    ElapsedTime,
                    ETA
                );

                try
                {
                    await SslOneReq(File, Ip, Port, data);
                    Thread.Sleep(300);
                }
                catch(Exception ex)
                {
                    Thread.Sleep(1000);
                    new Core.DataLoggerWriter(Guid.NewGuid(), "Internal Error", ex.ToString());
                    Console.Write(ex.ToString());
                }

            }
                
        }


        public static async Task SslOneReq(string req, string Ip, string Port, string data)
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
                sendMsg(str, sslStream);
                rsp = readMsg(sslStream, client);
                int n = 0;
                while (rsp == string.Empty)
                {
                    rsp = readMsg(sslStream, client);
                    if (n >= 1024) { break; }
                    n += 1;
                }
                //Console.WriteLine("{0}: {1}: {2}",uuid, str, rsp) ;
                new Core.DataLoggerWriter(uuid, str, rsp);

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
