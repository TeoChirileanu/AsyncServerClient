using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TcpServer
{
    public static class Server
    {
        private const int Port = 1337;
        private const string Message = "Hello World";
        private static TcpListener _server;

        public static async Task Main()
        {
            try
            {
                _server = TcpListener.Create(Port);
                _server.Start();
                Console.WriteLine($"Server started on port {Port}");
                await Run(_server);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            finally
            {
                _server.Stop();
                Console.WriteLine("Stopped server");
            }
        }

        private static async Task Run(TcpListener server)
        {
            Console.WriteLine("Server running...");
            while (true)
            {
                Console.WriteLine("Waiting a client to connect...");
                var client = await server.AcceptTcpClientAsync();
                Console.WriteLine("Established connection with client");
                await using var stream = client.GetStream();
                var data = Encoding.ASCII.GetBytes(Message);
                await stream.WriteAsync(data, 0, data.Length);
                Console.WriteLine($"Sent {Message} to client");
                await stream.FlushAsync();
                Console.WriteLine("Flushing the toilet");
                client.Close();
                Console.WriteLine("Closed connection with client");
            }
        }
    }
}