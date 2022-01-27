using Microsoft.EntityFrameworkCore;
using SimpleMessenger.Core.Model;
using SimpleMessenger.Server.Model;

namespace SimpleMessenger.Server;

static class Extensions
{
    public static User2? GetOne(this IQueryable<User2> users, Func<User2, bool> func)
        => users.Include(u => u.Contacts).FirstOrDefault(func);

    public static User2? GetOneById(this IQueryable<User2> users, int id)
        => users.GetOne(u => u.UID == id);

    public static User2? GetOneByUsername(this IQueryable<User2> users, string login) 
        => users.GetOne(u => u.Username == login);

    public static User2? GetOneByToken(this IQueryable<User2> users, Token token) 
        => users.GetOne(u => u.Token == token);
}