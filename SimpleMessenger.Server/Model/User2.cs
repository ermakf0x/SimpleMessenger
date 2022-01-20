using SimpleMessenger.Core;
using SimpleMessenger.Core.Model;

namespace SimpleMessenger.Server.Model;

class User2
{
    public int Id { get; set; }
    public Token CurrentToken { get; set; }
    public string Name { get; set; }

    public string UserName { get; set; }
    public string Password { get; set; }
    public DateTime RegTime { get; set; }

    public MainUser ToClientUser()
    {
        return new MainUser
        {
            Id = Id,
            Name = Name,
            Token = CurrentToken,
        };
    }
}
