using Microsoft.EntityFrameworkCore;

namespace SimpleMessenger.Server;

static class Program
{
    static void Main(string[] args)
    {
        //using var ds = new DataStorage();
        //var chats = ds.Chats.Include(c => c.Members)
        //                    .Include(c => c.Messages)
        //                    .ToList();

        //foreach (var chat in chats)
        //{
        //    ds.Chats.Remove(chat);
        //}

        //var msgs = ds.Message.Include(m => m.Sender).ToList();

        //foreach (var m in msgs)
        //{
        //    ds.Message.Remove(m);
        //}
        //ds.SaveChanges();



        Logger.Initialize();
        try
        {
            Server.Instance.Run();
        }
        catch (Exception e)
        {
            Logger.Fatal(e);
        }
    }
}
