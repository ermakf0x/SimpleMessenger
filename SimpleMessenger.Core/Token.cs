using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SimpleMessenger.Core;

[JsonConverter(typeof(TokenToJsonConverter))]
public struct Token : IEquatable<Token>
{
    Guid _data;

    static readonly Token _empty = new() { _data = Guid.Empty };
    public static Token Empty => _empty;

    public static Token New() => new() { _data = Guid.NewGuid(), };
    public bool Equals(Token other) => _data == other._data;
    public override bool Equals(object obj) => obj is Token token && Equals(token);
    public override int GetHashCode() => _data.GetHashCode();
    public override string ToString() => _data.ToString();

    public static Token Parse(string str) => new() { _data = Guid.Parse(str) };

    public static bool operator ==(Token left, Token right) => left.Equals(right);
    public static bool operator !=(Token left, Token right) => !(left == right);

    class TokenToJsonConverter : JsonConverter<Token>
    {
        public override Token Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return new Token { _data = Guid.Parse(reader.GetString()) };
        }
        public override void Write(Utf8JsonWriter writer, Token value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }
}
