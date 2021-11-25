namespace SimpleMessenger.Core
{
    public class User
    {
        public uint ID { get; set; }
        public string Name { get; set; }
        public UserData Data { get; set; }

        public override string ToString() => Name;
    }
}
