using SimpleMessenger.Core.Model;
using SimpleMessenger.Server.Model;

namespace SimpleMessenger.Server;

static class LocalDb
{
    public static User2 CreateNewUser(string username, string password, string name)
    {
        using var db = new DataStorage();

        if(db.Users.GetOneByUsername(username) is not null)
        {
            throw new UserNameIsTakenException(username);
        }

        var newUser = new User2
        {
            Token = Token.New(),
            Name = name,
            Username = username,
            Password = password,
            RegTime = DateTime.Now,
        };
        db.Users.Add(newUser);
        db.SaveChanges();

        return newUser;
    }

    public static void Update(User2 user)
    {
        using var db = new DataStorage();
        db.Users.Update(user);
        db.SaveChanges();
    }

    public static User2? GetUser(Func<User2, bool> func)
    {
        using var db = new DataStorage();
        return db.Users.GetOne(func);
    }
    public static User2? GetUserById(int id)
    {
        using var db = new DataStorage();
        return db.Users.GetOneById(id);
    }
    public static User2? GetUserByUsername(string username)
    {
        using var db = new DataStorage();
        return db.Users.GetOneByUsername(username);
    }
    public static User2? GetUserByToken(Token token)
    {
        using var db = new DataStorage();
        return db.Users.GetOneByToken(token);
    }
}