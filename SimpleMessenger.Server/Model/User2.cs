using SimpleMessenger.Core.Model;
using System.ComponentModel.DataAnnotations.Schema;

namespace SimpleMessenger.Server.Model;

class User2 : MainUser
{
    public string Username { get; set; }
    public string Password { get; set; }
    public DateTime RegTime { get; set; }
    public ICollection<User2> Contacts { get; set; }


    [NotMapped]
    public ClientHandler? Handler { get; set; }

    public MainUser GetAsMainUser() => new()
    {
        Token = Token,
        UID = UID,
        Name = Name,
    };
    public User GetAsUser() => new()
    {
        UID = UID,
        Name = Name,
    };


}
