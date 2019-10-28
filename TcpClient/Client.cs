using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using NetworkClient.Properties;

namespace NetworkClient
{
    public static class Client
    {
        private const int Port = 1337;
        private static readonly IPAddress IpAddress = IPAddress.Parse("127.0.0.1");

        public static async Task Main()
        {
            try
            {
                using var client = new TcpClient();
                await Run(client);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private static async Task Run(TcpClient client)
        {
            Console.WriteLine(Resources.Connecting);
            await client.ConnectAsync(IpAddress, Port);
            Console.WriteLine(Resources.Connected);

            await using var stream = client.GetStream();
            var rawData = new Memory<byte>(new byte[1024 * 1000 * 500]);
            var bytesRead = await stream.ReadAsync(rawData);
            Console.WriteLine(Resources.ReadData, bytesRead);
        }
    }
}