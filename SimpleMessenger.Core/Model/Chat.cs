using System.ComponentModel.DataAnnotations.Schema;

namespace SimpleMessenger.Core.Model;

public sealed class Chat
{
    readonly List<ChunkChat> _chunks = new();

    public int Id { get; init; }
    public int Hash { get; private set; }
    public DateTime LastTimeModified { get; set; } = DateTime.Now;
    public IReadOnlyList<User> Members { get; init; } = new List<User>(2);
    public IReadOnlyCollection<ChunkChat> Chunks => _chunks;
    [NotMapped]
    public int MessageCount
    {
        get
        {
            var count = 0;
            foreach (var chunk in _chunks)
                count += chunk.Count;
            return count;
        }
    }

    public Chat()
    {
        Hash = HashCode.Combine(Id);
    }

    public void AddMessage(Message message)
    {
        var now = DateTime.Now;
        var date = DateOnly.FromDateTime(now);
        var lastChunk = _chunks.LastOrDefault();

        if (lastChunk is null)
        {
            lastChunk = new ChunkChat { CreationTime = date };
            _chunks.Add(lastChunk);
        }
        else if (lastChunk.CreationTime != date)
        {
            lastChunk = new ChunkChat { CreationTime = date };
            _chunks.Add(lastChunk);
        }

        lastChunk.Add(message);
        LastTimeModified = now;
        Hash = HashCode.Combine(Hash, lastChunk);
    }

    public override int GetHashCode() => Hash;
    public override string ToString() => $"{Members[0]?.Name} - {Members[1]?.Name}";
}