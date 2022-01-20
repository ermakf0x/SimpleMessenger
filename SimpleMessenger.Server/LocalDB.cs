﻿using SimpleMessenger.Core;
using SimpleMessenger.Core.Model;
using SimpleMessenger.Server.Model;

namespace SimpleMessenger.Server;

static class LocalDb
{
    public static User2 New(string login, string password, string name)
    {
        using var db = new DataStorage();

        if(db.Users.GetOneByLogin(login) is not null)
        {
            throw new UserNameIsTakenException(login);
        }

        var newUser = new User2
        {
            CurrentToken = Token.New(),
            Name = name,
            UserName = login,
            Password = password,
            RegTime = DateTime.Now,
        };
        db.Users.Add(newUser);
        db.SaveChanges();

        return newUser;
    }

    public static void Update(User2 user)
    {
        var db = new DataStorage();
        db.Users.Update(user);
        db.SaveChanges();
    }

    public static User2? Get(Func<User2, bool> predicate)
    {
        using var db = new DataStorage();
        return db.Users.FirstOrDefault(predicate);
    }
    public static User2? GetById(int id)
    {
        using var db = new DataStorage();
        return db.Users.GetOneById(id);
    }
    public static User2? GetByLogin(string login)
    {
        using var db = new DataStorage();
        return db.Users.GetOneByLogin(login);
    }
    public static User2? GetByToken(Token token)
    {
        using var db = new DataStorage();
        return db.Users.GetOneByToken(token);
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