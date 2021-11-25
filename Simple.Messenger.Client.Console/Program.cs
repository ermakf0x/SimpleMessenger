using System;
using System.Buffers;
using System.Net.Sockets;
using System.Text;

namespace Simple.Messenger.Client.ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new TcpClient("127.0.0.1", 7777);
            var stream = client.GetStream();

            var memory = MemoryPool<byte>.Shared;
            Span<byte> buffer = memory.Rent(1024).Memory.Span;
            while (true)
            {
                var message = Console.ReadLine();
                Console.Clear();
                if (string.IsNullOrWhiteSpace(message)) continue;

                var count = Encoding.UTF8.GetBytes(message, buffer);
                stream.Write(buffer);
            }
        }
    }
}
