using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace SimpleMessenger.Core.Model;

public class ChunkChat : ICollection<Message>, IReadOnlyCollection<Message>
{
    readonly List<Message> _messages = new();

    [Key]
    public DateOnly CreationTime { get; init; }
    public DateTime LastTimeModified { get; private set; }
    public int Hash { get; private set; }
    public int Count => _messages.Count;

    public ChunkChat()
    {
        Hash = HashCode.Combine(CreationTime);
    }

    public void Add(Message message)
    {
        _messages.Add(message);
        LastTimeModified = DateTime.Now;
        Hash = HashCode.Combine(Hash, message);
    }
    public bool Remove(Message message)
    {
        var res = _messages.Remove(message);
        if (res)
        {
            LastTimeModified = DateTime.Now;
            // Сомнительное решение, нужно будет переписать
            Hash = HashCode.Combine(Hash, message);
        }
        return res;
    }
    public void Clear()
    {
        _messages.Clear();
        LastTimeModified = DateTime.Now;
        Hash = HashCode.Combine(CreationTime);
    }
    public bool Contains(Message item) => _messages.Contains(item);

    public void CopyTo(Message[] array, int arrayIndex) => _messages.CopyTo(array, arrayIndex);
    public IEnumerator<Message> GetEnumerator() => _messages.GetEnumerator();
    bool ICollection<Message>.IsReadOnly => ((ICollection<Message>)_messages).IsReadOnly;
    IEnumerator IEnumerable.GetEnumerator() => _messages.GetEnumerator();

    public override int GetHashCode() => Hash;
    public override string ToString() 
        => $"CreationTime: {CreationTime:hh:mm:ss}; Messages: {_messages.Count}; Hash: {Hash}";
}