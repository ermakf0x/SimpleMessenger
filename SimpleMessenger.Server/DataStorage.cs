using Microsoft.EntityFrameworkCore;
using SimpleMessenger.Core.Model;
using SimpleMessenger.Server.Model;

namespace SimpleMessenger.Server;

class DataStorage : DbContext
{
    public string DbPath { get; }
    public DbSet<User2> Users { get; set; }

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
        builder.Entity<User2>()
            .HasKey(u => u.UID);


        // Конвертируем Token в String и обратно
        builder.Entity<User2>()
            .Property(u => u.Token)
            .HasConversion(t => t.ToString(), s => Token.Parse(s))
            .HasMaxLength(36);
    }
}
/*
    Install-Package Microsoft.EntityFrameworkCore.Tools
    Add-Migration InitialCreate                             - добавить новую миграци для Db
    Update-Database                                         - обновить Db
*/