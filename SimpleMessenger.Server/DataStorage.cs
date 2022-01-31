using Microsoft.EntityFrameworkCore;
using SimpleMessenger.Core.Model;
using SimpleMessenger.Server.Model;

namespace SimpleMessenger.Server;

class DataStorage : DbContext
{
    public string DbPath { get; }
    public DbSet<User2> Users { get; set; }
    public DbSet<Chat> Chats { get; set; }

    public DataStorage()
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
    }
}
/*
 *  Install-Package Microsoft.EntityFrameworkCore.Tools
 *  Add-Migration InitialCreate                             - добавить новую миграци для Db
 *  Update-Database                                         - обновить Db
*/