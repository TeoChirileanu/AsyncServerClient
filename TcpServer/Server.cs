using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using NetworkServer.Properties;

namespace NetworkServer
{
    public static class Server
    {
        private const int Port = 1337;

        public static async Task Main()
        {
            var server = TcpListener.Create(Port);
            try
            {
                server.Start();
                Console.WriteLine(Resources.Started, Port);
                await Run(server);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            finally
            {
                server.Stop();
                Console.WriteLine(Resources.Stopped);
            }
        }

        private static async Task Run(TcpListener server)
        {
            Console.WriteLine(Resources.Running);
            while (true)
            {
                Console.WriteLine(Resources.Waiting);
                var client = await server.AcceptTcpClientAsync();
                Console.WriteLine(Resources.Established);
                await using var stream = client.GetStream();
                var messageAsBytes = await GetMessageAsBytes();
                await stream.WriteAsync(messageAsBytes);
                Console.WriteLine(Resources.Sent, messageAsBytes.Length);
                client.Close();
                Console.WriteLine(Resources.Closed);
            }
        }

        private static async Task<byte[]> GetMessageAsBytes()
        {
            const string file = @"C:\rider.exe";
            return await File.ReadAllBytesAsync(file);
        }
    }
}