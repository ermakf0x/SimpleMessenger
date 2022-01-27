using Microsoft.EntityFrameworkCore;
using SimpleMessenger.Core.Model;
using SimpleMessenger.Server.Model;

namespace SimpleMessenger.Server;

class DataStorage : DbContext
{
    public string DbPath { get; }
    public DbSet<User2> Users { get; set; }
    //public DbSet<Contact> Contacts { get; set; }

    public DataStorage()
    {
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        DbPath = $"{path}{Path.DirectorySeparatorChar}users.db"; // ..AppData/Local/
    }

    protected override void OnConfiguring(DbContextOptionsBuilder builder)
    {
        builder.UseSqlite($"Data Source={DbPath}");
    }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<User2>(user =>
        {
            user.HasKey(u => u.UID);

            // Конвертируем Token в String и обратно
            user.Property(u => u.Token)
            .HasConversion(t => t.ToString(), s => Token.Parse(s))
            .HasMaxLength(36);
        });

        builder.Entity<Contact>().HasKey(c => new { c.CurrentId, c.FriendId });

        builder.Entity<Contact>()
            .HasOne(pt => pt.Friend)
            .WithMany() // <--
            .HasForeignKey(pt => pt.FriendId);
            //.OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Contact>()
            .HasOne(pt => pt.Current)
            .WithMany(t => t.Contacts)
            .HasForeignKey(pt => pt.CurrentId);
    }
}
/*

    Install-Package Microsoft.EntityFrameworkCore.Tools
    Add-Migration InitialCreate                             - добавить новую миграци для Db
    Update-Database                                         - обновить Db

            migrationBuilder.InsertData( "Users",
                new string[] { "Username", "Password", "RegTime", "Name", "Token" },
                new object[] { "User", "qwerty1234", DateTime.Now, "user", Token.Empty.ToString() });

            migrationBuilder.InsertData( "Users",
                new string[] { "Username", "Password", "RegTime", "Name", "Token" },
                new object[] { "User2", "qwerty1234", DateTime.Now, "user", Token.Empty.ToString() });
*/