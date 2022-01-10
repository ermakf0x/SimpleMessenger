using System;

namespace SimpleMessenger.Core;

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

    public static bool operator ==(Token left, Token right) => left.Equals(right);
    public static bool operator !=(Token left, Token right) => !(left == right);
}
