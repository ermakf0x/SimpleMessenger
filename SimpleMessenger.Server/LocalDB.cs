using SimpleMessenger.Core;
using SimpleMessenger.Server.Model;

namespace SimpleMessenger.Server;

static class LocalDb
{
    public static User2 New(string name)
    {
        using var db = new UsersContext();
        var newUser = new User2
        {
            UserName = name,
            CurrentToken = Token.New()
        };
        newUser = db.Users.Add(newUser).Entity;
        db.SaveChanges();

        return newUser;
    }
    public static void Add(User2 user)
    {
        using var db = new UsersContext();
        db.Users.Add(user);
        db.SaveChanges();
    }

    public static User2? Get(Func<User2, bool> predicate)
    {
        using var db = new UsersContext();
        return db.Users.FirstOrDefault(predicate);
    }
    public static User2? GetById(int id)
    {
        using var db = new UsersContext();
        return db.Users.SingleOrDefault(u => u.Id == id);
    }

    public static bool Contains(Func<User2, bool> predicate)
    {
        return Get(predicate) != null;
    }
    public static bool ContainsByToken(in Token token)
    {
        var t = token;
        return Contains(u => u.CurrentToken == t);
    }
}