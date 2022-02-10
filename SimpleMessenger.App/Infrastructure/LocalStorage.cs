using Microsoft.EntityFrameworkCore;
using SimpleMessenger.Core.Model;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SimpleMessenger.App.Infrastructure;

class LocalStorage : DbContext
{
    static string _dbPath;
    public DbSet<User> Contacts { get; set; }
    public DbSet<Chat> Chats { get; set; }

    public Task InitAsync()
    {
        _dbPath = DbUtils.GenerateDbPath(SimpleMessenger.App.Client.User);
        if(!File.Exists(_dbPath))
        {
            return Database.EnsureCreatedAsync();
        }
        return Task.CompletedTask;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder builder)
        => builder.UseSqlite($"Data Source={_dbPath};");

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Chat>(chat =>
        {
            chat.Navigation(c => c.Members).AutoInclude();
            chat.Navigation(c => c.Chunks).AutoInclude();
        });
    }
}

static class DbUtils
{
    public static string GenerateDbPath(User user)
    {
        ArgumentNullException.ThrowIfNull(user, nameof(user));

        var sep = Path.DirectorySeparatorChar;
        var folder = "SimpleMessenger";
        var path = $"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}{sep}{folder}";
        string fileName;

        {
            Span<byte> bufIn = stackalloc byte[user.Username.Length * 4];
            Span<byte> bufOut = stackalloc byte[128];
            var encoding = Encoding.ASCII;
            var count = encoding.GetBytes(user.Username, bufIn);
            count = MD5.HashData(bufIn[..count], bufOut);

            var sb = new StringBuilder(32);
            foreach (var b in bufOut[..count])
                sb.Append(b.ToString("X2"));
            fileName = sb.ToString();
        }

        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);

        return $"{path}{sep}{fileName}.db"; // ..AppData/Local/SimpleMessenger/
    }
}