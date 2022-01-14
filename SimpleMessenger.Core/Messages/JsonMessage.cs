using System.Text.Json;

namespace SimpleMessenger.Core.Messages;
public sealed class JsonMessage : IMessage, IResponse
{
    string? _jsonString;
    object? _data;

    public MessageType MessageType => MessageType.Json;
    public bool HasData => !string.IsNullOrEmpty(_jsonString) || _data != null;

    public JsonMessage(object? data = null) => _data = data;

    public T? GetAs<T>()
    {
        if (TryGetAs(out T? data)) return data;
        throw new Exception();
    }
    public bool TryGetAs<T>(out T? data)
    {
        if (!HasData)
        {
            data = default;
            return false;
        }

        if(_data == null)
        {
            T temp = JsonSerializer.Deserialize<T>(_jsonString);
            _data = temp;
            data = temp;
            return true;
        }

        data = (T)_data;
        return true;
    }
    void IMessage.Write(DataWriter writer)
    {
        var json = _data == null ? string.Empty : JsonSerializer.Serialize(_data);
        writer.Write(json);
    }
    void IMessage.Read(DataReader reader)
    {
        _jsonString = reader.ReadString();
    }

    public override string ToString()
    {
        return _data?.ToString() ?? _jsonString ?? string.Empty;
    }
}
