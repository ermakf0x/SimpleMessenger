namespace SimpleMessenger.Server;

sealed class Logger
{
    static readonly string[] _logLevelsStr =
    {
        "DEBUG",
        "TRACE",
        "INFO",
        "WARNING",
        "ERROR",
        "FATAL",
    };

    public static Logger Current { get; private set; }

    Logger() { }

    public static bool Initialize()
    {
        if (Current is not null) return false;
        Current = new Logger();
        return true;
    }

    public static void Debug(string msg) => Current.Write(LogLevel.Debug, msg);
    public static void Trace(string msg) => Current.Write(LogLevel.Trace, msg, ConsoleColor.Gray, Console.BackgroundColor);

    public static void Info(string msg) => Current.Write(LogLevel.Info, msg);
    public static void Info(string msg, ConsoleColor foregroundColor) => Current.Write(LogLevel.Info, msg, foregroundColor, Console.BackgroundColor);

    public static void Warning(string msg) => Current.Write(LogLevel.Warning, msg);

    public static void Error(Exception e) => Error(e.ToString());
    public static void Error(string msg) => Current.Write(LogLevel.Error, msg, ConsoleColor.Red, Console.BackgroundColor);

    public static void Fatal(Exception e) => Fatal(e.ToString());
    public static void Fatal(string msg) => Current.Write(LogLevel.Fatal, msg, Console.ForegroundColor, ConsoleColor.Red);

    public void Write(LogLevel level, string msg, ConsoleColor foregroundColor, ConsoleColor backgroundColor)
    {
        var fc = Console.ForegroundColor;
        var bc = Console.BackgroundColor;
        Console.ForegroundColor = foregroundColor;
        Console.BackgroundColor = backgroundColor;

        Write(level, msg);

        Console.ForegroundColor = fc;
        Console.BackgroundColor = bc;
    }
    public void Write(LogLevel level, string msg)
    {
        Console.WriteLine($"[{DateTime.Now}] {LLToString(level)}: {msg}");
    }

    static string LLToString(LogLevel level)
    {
        return _logLevelsStr[(byte)level];
    }
}

enum LogLevel : byte
{
    Debug,
    Trace,
    Info,
    Warning,
    Error,
    Fatal
}