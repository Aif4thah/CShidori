﻿using System;
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

namespace CShidori.NetworkTest
{
    internal class TcpFuzz
    {
        public static async void TcpFuzzAsync(string File, string Ip, string Port, string data)
        {
            while (true)
            {
                try
                {
                    await SendOneReq(File, Ip, Port, data);
                }
                catch
                {
                    Thread.Sleep(1000);
                }

            }

        }


        public static async Task SendOneReq(string File, string Ip, string Port, string data)
        {
            Guid uuid;
            TcpClient client = new TcpClient(Ip, int.Parse(Port));
            var stream = client.GetStream();

            string req = System.IO.File.ReadAllText(File);
            string rsp = string.Empty;

            uuid = Guid.NewGuid();
            Core.Mutation mut = new Core.Mutation(1, req, data);
            foreach (string str in mut.Output) // Convert string[] mut.Output to string str
            {
                sendMsg(str, stream);
                rsp = readMsg(stream);
                int n = 0;
                while (true)
                {
                    rsp += readMsg(stream);
                    if (n >= 4096) { break; }
                    n += 1;
                }
                Console.WriteLine("{0}:\n{1}\n---\n{2}", uuid, str, rsp);
                new Core.DataLoggerWriter(uuid, str, rsp);

            }
            client.Close();

        }


        public static string readMsg(Stream stream)
        {
            byte[] buffer = new byte[4096];
            int bytesRead = stream.Read(buffer, 0, buffer.Length);
            return Encoding.UTF8.GetString(buffer, 0, bytesRead);
        }
        public static void sendMsg(string message, Stream stream)
        {

            stream.Write(Encoding.UTF8.GetBytes(message), 0, Encoding.UTF8.GetBytes(message).Length);
        }
    }
}
