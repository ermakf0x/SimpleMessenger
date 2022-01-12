using SimpleMessenger.Core;

namespace SimpleMessenger.Server.Model;

class User2
{
    public int Id { get; set; }
    public Token CurrentToken { get; set; }
    public string UserName { get; set; }
    public string? NickName { get; set; }
}