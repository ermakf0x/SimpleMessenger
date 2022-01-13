using SimpleMessenger.Core;

namespace SimpleMessenger.Server.Model;

class User2
{
    public int Id { get; set; }
    public Token CurrentToken { get; set; }
    public string Name { get; set; }

    public string Login { get; set; }
    public string Password { get; set; }
    public DateTime RegTime { get; set; }

    public User ToClientUser()
    {
        return new User
        {
            Id = Id,
            Name = Name,
            Token = CurrentToken,
        };
    }
}
