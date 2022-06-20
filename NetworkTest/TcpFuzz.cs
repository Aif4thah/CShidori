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
    internal class TcpFuzz
    {
        public static async void TcpFuzzAsync(string File, string Ip, string Port, string data)
        {
            string LogFile = Ip + "-" + Guid.NewGuid().ToString();
            Console.WriteLine("[*] Log file will be: {0}", LogFile);

            Console.WriteLine("[*] Reading File: {0}", File);
            string req = System.IO.File.ReadAllText(File);

            Console.WriteLine("[*] Send Original request");
            await SendOneReq(req, Ip, Port, data, LogFile);

            Console.WriteLine("[*] Initialize Fuzzing");
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            double TotalReq = req.Length * 200;
            double ElapsedTime;

            Console.WriteLine("[*] Start Fuzzing for {0} requests", TotalReq);
            int i = 1;
            while(i <= TotalReq)
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
                    foreach( string str in mut.Output)
                    {
                        await SendOneReq(str, Ip, Port, data, LogFile);
                        i++;
                        //Thread.Sleep(300); /* use this to avoid Firewall Ban or DOS
                    }

                }
                catch(Exception ex)
                {
                    Thread.Sleep(100); 
                    new DataLogWriter(LogFile, Guid.NewGuid(), "Internal Error", ex.ToString());
                    Console.Write(ex.ToString());
                }

            }

        }


        private static async Task SendOneReq(string req, string Ip, string Port, string data, string LogFile)
        {

            Guid uuid = Guid.NewGuid();                    

            TcpClient client = new TcpClient(Ip, int.Parse(Port));
            var stream = client.GetStream();
            stream.ReadTimeout = 2000;

            string rsp = string.Empty;

            sendMsg(req, stream);

            for(int n = 0; n < 5; n++){ rsp += readMsg(stream);}

            new DataLogWriter(LogFile, uuid, req, rsp);

            client.Close();
        }


        private static string readMsg(Stream stream)
        {
            //Console.WriteLine("[reciv]");
            byte[] buffer = new byte[4096];
            int bytesRead = stream.Read(buffer, 0, buffer.Length);
            return Encoding.UTF8.GetString(buffer, 0, bytesRead);
        }
        private static void sendMsg(string message, Stream stream)
        {
            //Console.WriteLine("[send]");
            stream.Write(Encoding.UTF8.GetBytes(message), 0, Encoding.UTF8.GetBytes(message).Length);
        }
    }
}
