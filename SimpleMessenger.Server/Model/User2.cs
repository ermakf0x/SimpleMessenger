using SimpleMessenger.Core.Model;
using System.ComponentModel.DataAnnotations.Schema;

namespace SimpleMessenger.Server.Model;

class User2 : MainUser
{
    public new Token Token { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public DateTime RegTime { get; set; }

    [NotMapped]
    public ClientHandler? Handler { get; set; }

    public MainUser GetMainUser()
    {
        return new MainUser
        {
            Token = Token,
            UID = UID,
            Name = Name,
        };
    }
    public User GetUser()
    {
        return new User
        {
            UID = UID,
            Name = Name,
        };
    }
}
