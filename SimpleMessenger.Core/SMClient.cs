using SimpleMessenger.Core.Messages;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace SimpleMessenger.Core;

public sealed class SMClient : IDisposable
{
    readonly TcpClient _tcpClient;
    readonly IPEndPoint _endPoint;
    NetworkChannel _channel;
    private bool disposedValue;

    public bool Connected => _tcpClient.Connected;

    public SMClient(string ip, int port)
    {
        _tcpClient = new TcpClient();
        _endPoint = new IPEndPoint(IPAddress.Parse(ip), port);
    }

    public async Task ConnectAsync()
    {
        await _tcpClient.ConnectAsync(_endPoint).ConfigureAwait(false);
        _channel = new NetworkChannel(_tcpClient.GetStream(), new MessageSerializer(Encoding.UTF8));
    }
    public async Task<IResponse> ConnectAsync(HelloServerMessage helloMessage)
    {
        ArgumentNullException.ThrowIfNull(helloMessage, nameof(helloMessage));

        await _tcpClient.ConnectAsync(_endPoint).ConfigureAwait(false);
        _channel = new NetworkChannel(_tcpClient.GetStream(), new MessageSerializer(Encoding.UTF8));

        return await SendAsync(helloMessage).ConfigureAwait(false);
    }

    public async Task<IResponse> SendAsync(IMessage message)
    {
        if (!Connected) return null; // TODO: Если нет соединения - бросать исключение

        await _channel.SendAsync(message).ConfigureAwait(false);
        while (true)
        {
            if (!_channel.MessageAvailable)
            {
                Thread.Sleep(1);
                continue;
            }
            return await _channel.ReceiveAsync().ConfigureAwait(false) as IResponse;
        }
    }
    public Task<IMessage?> ReceiveAsync()
    {
        if (Connected) return _channel.ReceiveAsync();
        return Task.FromResult<IMessage?>(null);
    }

    private void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                // TODO: освободить управляемое состояние (управляемые объекты)
                if(_tcpClient != null)
                {
                    _tcpClient.Close();
                }
            }

            // TODO: освободить неуправляемые ресурсы (неуправляемые объекты) и переопределить метод завершения
            // TODO: установить значение NULL для больших полей
            disposedValue = true;
        }
    }

    // // TODO: переопределить метод завершения, только если "Dispose(bool disposing)" содержит код для освобождения неуправляемых ресурсов
    // ~SMClient()
    // {
    //     // Не изменяйте этот код. Разместите код очистки в методе "Dispose(bool disposing)".
    //     Dispose(disposing: false);
    // }

    public void Dispose()
    {
        // Не изменяйте этот код. Разместите код очистки в методе "Dispose(bool disposing)".
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}