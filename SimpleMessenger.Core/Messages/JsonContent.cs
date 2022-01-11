using System.Text.Json;
using System;
using System.IO;

namespace SimpleMessenger.Core.Messages;
public class JsonContent : IResponse
{
    string _jsonString;
    object _data;

    public MessageType MessageType => MessageType.JsonContent;
    public bool HasData => !string.IsNullOrEmpty(_jsonString) || _data != null;

    public JsonContent(object data = null) => _data = data;

    public T GetAs<T>()
    {
        if (TryGetAs<T>(out T data)) return data;
        throw new Exception();
    }
    public bool TryGetAs<T>(out T data)
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

    public void Read(Stream stream)
    {
        _jsonString = stream.ReadString();
    }

    public void Write(Stream stream)
    {
        var json = _data == null ? string.Empty : JsonSerializer.Serialize(_data);
        stream.Write(json);
    }
}
