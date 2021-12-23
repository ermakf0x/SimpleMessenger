using System;
using System.Buffers;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace SimpleMessenger.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            var server = new TcpListener(IPAddress.Any, 7777);
            server.Start();
            Console.WriteLine("Server is started!");

            while (true)
            {
                var tcpClient = server.AcceptTcpClient();
                Console.WriteLine("New connection to server");
                var stream = tcpClient.GetStream();

                while (tcpClient.Connected)
                {
                    var memory = MemoryPool<byte>.Shared;
                    var buffer = memory.Rent(1024).Memory.Span;
                    var count = stream.Read(buffer);
                    Console.WriteLine(Encoding.UTF8.GetString(buffer));
                }
            }
        }
    }
}
