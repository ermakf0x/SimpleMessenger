using SimpleMessenger.Core;
using System.Collections.Concurrent;

namespace SimpleMessenger.Server;

static class LocalDB
{
    static readonly ConcurrentBag<User2> _users = new();

    public static void Add(User2 user) => _users.Add(user);
    public static User2? Get(Func<User2, bool> predicate) => _users.Where(predicate).FirstOrDefault();
    public static User2? GetById(int id) => Get(u => u.Data.Id == id);
    public static User2? GetByToken(Guid token) => Get(u => u.Token == token);
    public static User2 New(string name)
    {
        var user = new User2
        {
            Data = new UserData { Name = name, Id = GetNextID() },
            Token = Guid.NewGuid()
        };
        _users.Add(user);
        return user;
    }
    public static IReadOnlyCollection<User2> GetUsers() => _users;

    static  int GetNextID()
    {
        var user = _users.Last();
        if (user == null) return 0;
        return user.Data.Id++;
    }
}

class User2 : User
{

}