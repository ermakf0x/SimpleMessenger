using SimpleMessenger.Core.Model;
using SimpleMessenger.Server.Model;

namespace SimpleMessenger.Server;

static class Extensions
{
    public static User2? GetOneById(this IQueryable<User2> users, int id) => users.FirstOrDefault(u => u.UID == id);
    public static User2? GetOneByLogin(this IQueryable<User2> users, string login) => users.FirstOrDefault(u => u.Username == login);
    public static User2? GetOneByToken(this IQueryable<User2> users, Token token) => users.FirstOrDefault(u => u.Token == token);
}