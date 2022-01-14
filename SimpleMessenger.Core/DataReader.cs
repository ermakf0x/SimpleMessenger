using System.Text;

namespace SimpleMessenger.Core;

public readonly struct DataReader
{
    readonly Stream _stream;
    readonly Encoding _encoding;

    public DataReader(Stream stream, Encoding encoding)
    {
        _stream = stream ?? throw new ArgumentNullException(nameof(stream));
        _encoding = encoding;
    }

    public T Read<T>() where T : unmanaged
    {
        return _stream.Read<T>();
    }
    public char[] ReadCharArray()
    {
        return _stream.ReadCharArray(_encoding);
    }

    public string ReadString()
    {
        return _stream.ReadString(_encoding);
    }

    public T[] ReadArray<T>() where T : unmanaged
    {
        return _stream.ReadArray<T>();
    }
}