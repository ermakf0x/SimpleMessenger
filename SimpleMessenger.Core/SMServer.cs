using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleMessenger.Core;

public class SMServer
{
    readonly TcpListener _tcpListener = new(IPAddress.Any, 7777);
    readonly List<Task> _newConnection = new();
    readonly ICommandSerializer _serializer = new CommandSerializer();

    public void Start()
    {
        _tcpListener.Start();
        Console.WriteLine("Server: Started!");

        while (true)
        {
            var tcpClient = _tcpListener.AcceptTcpClient();
            Console.WriteLine($"Server: New connection to server: {tcpClient.Client.RemoteEndPoint}");
            AddNewConnection(tcpClient);
        }
    }

    void AddNewConnection(TcpClient tcpClient)
    {
        Task t = null;

        t = Task.Factory.StartNew(() =>
        {
            using var stream = tcpClient.GetStream();

            while (tcpClient.Connected)
            {
                try
                {

                    if(!stream.DataAvailable)
                    {
                        Thread.Sleep(1);
                        continue;
                    }
                    var command = _serializer.Desirialize(stream);
                    Console.WriteLine($"New message: {command}");
                }
                catch (Exception ex)
                {
                    if (ex.GetType() == typeof(SocketException))
                    {
                        Console.WriteLine($"{tcpClient.Client.RemoteEndPoint} disconnected");
                        break;
                    }
                    Console.WriteLine(ex.Message);
                    break;
                }
            }

            _newConnection.Remove(t);
        });
        _newConnection.Add(t);
    }
}
