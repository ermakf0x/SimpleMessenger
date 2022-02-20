using System.ComponentModel.DataAnnotations.Schema;

namespace SimpleMessenger.Core.Model;

public sealed class Chat
{
    readonly List<ChunkChat> _chunks = new();

    public int Id { get; init; }
    public int Hash { get; set; }
    public DateTime LastTimeModified { get; set; } = DateTime.Now;
    public int FirstUserID { get; init; } = -1;
    public int SecondUserID { get; init; } = -1;
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
        InitHash();
    }
    public Chat(User user, User user2)
    {
        FirstUserID = user.UID;
        SecondUserID = user2.UID;
        InitHash();
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
    public override string ToString()
    {
        return $"Id: {Id}; Hash: {Hash}; Members: ({FirstUserID} - {SecondUserID})";
    }

    void InitHash()
    {
        Hash = HashCode.Combine(Id);
    }
}