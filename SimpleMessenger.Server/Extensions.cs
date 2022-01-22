using SimpleMessenger.Core;
using SimpleMessenger.Core.Model;
using SimpleMessenger.Server.Model;

namespace SimpleMessenger.Server;

static class Extensions
{
    public static bool IsAuth<TMsg>(this TMsg message, ClientHandler client)
        where TMsg : IMessage
    {
        if (client.CurrentUser != null) return true;
        if (message is ITokenable msg && msg.Token != Token.Empty)
        {
            return LocalDb.ContainsByToken(msg.Token);
        }

        return false;
    }

    public static User2? GetOneById(this IQueryable<User2> users, int id) => users.FirstOrDefault(u => u.UID == id);
    public static User2? GetOneByLogin(this IQueryable<User2> users, string login) => users.FirstOrDefault(u => u.Username == login);
    public static User2? GetOneByToken(this IQueryable<User2> users, Token token) => users.FirstOrDefault(u => u.Token == token);
    public static bool ContainsByToken(this IQueryable<User2> users, Token token) => users.GetOneByToken(token) is not null;
    public static bool ContainsByLogin(this IQueryable<User2> users, string login) => users.GetOneByLogin(login) is not null;
}