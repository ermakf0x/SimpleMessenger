using Newtonsoft.Json;
using System.IO;

namespace SimpleMessenger.Core.Messages;
public class JsonContent : IResponse
{
    public MessageType MessageType => MessageType.JsonContent;
    public object Data { get; set; }
    public bool HaveData => Data != null;

    public JsonContent(object data = null) => Data = data;

    public bool TryGetAs<T>(out T data)
    {
        if (!HaveData)
        {
            data = default;
            return false;
        }

        data = (T)Data;
        return true;
    }

    public void Read(Stream stream)
    {
        Data = JsonConvert.DeserializeObject(stream.ReadString());
    }

    public void Write(Stream stream)
    {
        stream.Write(JsonConvert.SerializeObject(Data));
    }
}
