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

    public async Task InitAsync(UserConfig config)
    {
        _dbPath = DbUtils.GenerateDbPath(config);
        if(!File.Exists(_dbPath))
        {
            await Database.EnsureCreatedAsync().ConfigureAwait(false);
        }
        await Task.WhenAll(Contacts.LoadAsync(), Chats.LoadAsync()).ConfigureAwait(false);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder builder)
        => builder.UseSqlite($"Data Source={_dbPath};");
}

static class DbUtils
{
    public static string GenerateDbPath(UserConfig config)
    {
        ArgumentNullException.ThrowIfNull(config, nameof(config));

        var sep = Path.DirectorySeparatorChar;
        var folder = "SimpleMessenger";
        var path = $"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}{sep}{folder}";
        string fileName;

        {
            Span<byte> bufIn = stackalloc byte[config.Username.Length * 4];
            Span<byte> bufOut = stackalloc byte[128];
            var encoding = Encoding.ASCII;
            var count = encoding.GetBytes(config.Username, bufIn);
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