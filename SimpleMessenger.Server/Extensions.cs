using SimpleMessenger.Core;
using SimpleMessenger.Core.Messages;
using SimpleMessenger.Server.Model;

namespace SimpleMessenger.Server;

static class Extensions
{
    public static bool IsAuth<TMsg>(this TMsg message, ServerClient client)
        where TMsg : IMessage
    {
        if (client.User != null) return true;
        if (message is Message msg && msg.Token != Token.Empty)
        {
            return LocalDb.ContainsByToken(msg.Token);
        }

        return false;
    }

    public static User2? GetOneById(this IQueryable<User2> users, int id) => users.FirstOrDefault(u => u.Id == id);
    public static User2? GetOneByLogin(this IQueryable<User2> users, string login) => users.FirstOrDefault(u => u.Login == login);
    public static User2? GetOneByToken(this IQueryable<User2> users, Token token) => users.FirstOrDefault(u => u.CurrentToken == token);
    public static bool ContainsByToken(this IQueryable<User2> users, Token token) => users.GetOneByToken(token) is not null;
    public static bool ContainsByLogin(this IQueryable<User2> users, string login) => users.GetOneByLogin(login) is not null;
}