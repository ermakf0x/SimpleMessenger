using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace SimpleMessenger.Core;

public readonly struct DataWriter
{
    readonly Stream _stream;
    readonly Encoding _encoding;

    public DataWriter(Stream stream, Encoding encoding)
    {
        _stream = stream ?? throw new ArgumentNullException(nameof(stream));
        _encoding = encoding;
    }

    public void Write<T>(T value) where T : unmanaged
    {
        _stream.Write(value);
    }
    public void Write<T>(ref T value) where T : unmanaged
    {
        _stream.Write(ref value);
    }
    public void Write<T>(T[] values) where T : unmanaged
    {
        _stream.Write(values);
    }
    public void Write<T>(Span<T> values) where T : unmanaged
    {
        _stream.Write(values);
    }
    public void Write(string value)
    {
        _stream.Write(value, _encoding);
    }
    public void Write(char[] value)
    {
        _stream.Write(value, _encoding);
    }
}
