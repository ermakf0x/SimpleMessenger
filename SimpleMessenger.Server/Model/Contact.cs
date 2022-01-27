namespace SimpleMessenger.Server.Model;

class Contact
{
    public int CurrentId { get; set; }
    public virtual User2 Current { get; set; }

    public int FriendId { get; set; }
    public virtual User2 Friend { get; set; }
}