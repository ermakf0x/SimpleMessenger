using SimpleMessenger.Core.Model;
using System.ComponentModel.DataAnnotations.Schema;

namespace SimpleMessenger.Server.Model;

class User2 : MainUser
{
    public string Password { get; set; }
    public DateTime RegDate { get; init; }
    public ICollection<User2> Contacts { get; } = new List<User2>();
    public ICollection<Chat> Chats { get; } = new List<Chat>();

    [NotMapped]
    public ClientHandler? Handler { get; set; }

    public MainUser GetAsMainUser() => new()
    {
        Token = Token,
        UID = UID,
        Name = Name,
        Username = Username
    };
    public User GetAsUser() => new()
    {
        UID = UID,
        Name = Name,
        Username = Username
    };
}