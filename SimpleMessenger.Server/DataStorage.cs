using Microsoft.EntityFrameworkCore;
using SimpleMessenger.Core.Model;
using SimpleMessenger.Server.Model;

namespace SimpleMessenger.Server;

class DataStorage : DbContext
{
    public string DbPath { get; }
    public DbSet<User2> Users { get; set; }
    public DbSet<Chat> Chats { get; set; }
    public DbSet<Message> Message { get; set; }

#pragma warning disable CS8618 // Поле, не допускающее значения NULL, должно содержать значение, отличное от NULL, при выходе из конструктора. Возможно, стоит объявить поле как допускающее значения NULL.
    public DataStorage()
#pragma warning restore CS8618 // Поле, не допускающее значения NULL, должно содержать значение, отличное от NULL, при выходе из конструктора. Возможно, стоит объявить поле как допускающее значения NULL.
    {
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        DbPath = $"{path}{Path.DirectorySeparatorChar}DataStorage.db"; // ..AppData/Local/
    }

    protected override void OnConfiguring(DbContextOptionsBuilder builder)
    {
        builder.UseSqlite($"Data Source={DbPath}");
    }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<User2>(user =>
        {
            // Конвертируем Token в String и обратно
            user.Property(u => u.Token)
            .HasConversion(t => t.ToString(), s => Token.Parse(s))
            .HasMaxLength(36);
        });

        builder.Entity<Chat>().Navigation(c => c.Members).AutoInclude();
        builder.Entity<Chat>().Navigation(c => c.Chunks).AutoInclude();
    }
}
/*
 *  Install-Package Microsoft.EntityFrameworkCore.Tools
 *  Add-Migration InitialCreate                             - добавить новую миграци для Db
 *  Update-Database                                         - обновить Db
*/