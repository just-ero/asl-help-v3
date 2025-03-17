using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace AslHelp.IO.Logging;

public abstract class Logger
{
    public LoggerVerbosity Verbosity { get; set; }
#if DEBUG
        = LoggerVerbosity.Detailed;
#endif

    public LogFormat OutputFormat { get; set; }
#if DEBUG
        = LogFormat.ShowTimestamp | LogFormat.ShowMember | LogFormat.ShowLocation;
#endif

    protected abstract void WriteLine(string output);

    private void LogInternal(string message, string member, string file, int line)
    {
        if ((OutputFormat & LogFormat.ShowTimestamp) != 0)
        {
            WriteLine($"[{DateTimeOffset.Now:yyyy-MM-dd HH:mm:ss.fff}] {message}");
        }
        else
        {
            WriteLine(message);
        }

        if ((OutputFormat & LogFormat.ShowMember) != 0)
        {
            if ((OutputFormat & LogFormat.ShowLocation) != 0)
            {
                WriteLine($"                             in {member} at {Path.GetFileName(file)}:{line}");
            }
            else
            {
                WriteLine($"                             in {member}");
            }
        }
        else
        {
            if ((OutputFormat & LogFormat.ShowLocation) != 0)
            {
                WriteLine($"                             at {Path.GetFileName(file)}:{line}");
            }
        }
    }

    public void Log(
        LoggerVerbosity verbosity,
        string message,
        [CallerMemberName] string member = "",
        [CallerFilePath] string file = "",
        [CallerLineNumber] int line = 0)
    {
        if (verbosity >= Verbosity)
        {
            LogInternal(message, member, file, line);
        }
    }

    public void LogDetail(
        string message,
        [CallerMemberName] string member = "",
        [CallerFilePath] string file = "",
        [CallerLineNumber] int line = 0)
    {
        Log(LoggerVerbosity.Detailed, message, member, file, line);
    }

    public void LogInfo(
        string message,
        [CallerMemberName] string member = "",
        [CallerFilePath] string file = "",
        [CallerLineNumber] int line = 0)
    {
        Log(LoggerVerbosity.Info, message, member, file, line);
    }

    public void LogWarning(
        string message,
        [CallerMemberName] string member = "",
        [CallerFilePath] string file = "",
        [CallerLineNumber] int line = 0)
    {
        Log(LoggerVerbosity.Warning, message, member, file, line);
    }

    public void LogCritical(
        string message,
        [CallerMemberName] string member = "",
        [CallerFilePath] string file = "",
        [CallerLineNumber] int line = 0)
    {
        Log(LoggerVerbosity.Critical, message, member, file, line);
    }
}
