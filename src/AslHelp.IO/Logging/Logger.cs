using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;

namespace AslHelp.IO.Logging;

public abstract class Logger
{
    public LoggerVerbosity Verbosity { get; set; }
    public LogFormatOptions OutputFormat { get; set; }

    protected abstract void WriteLine(string output);

    private void LogInternal(string message, string member, string file, int line)
    {
        StringBuilder outputBuilder = new();

        if ((OutputFormat & LogFormatOptions.ShowTimestamp) != 0)
        {
            outputBuilder.Append($"[{DateTimeOffset.Now:yyyy-MM-dd HH:mm:ss.fff}] ");
        }

        outputBuilder.Append(message);

        if ((OutputFormat & LogFormatOptions.ShowMember) != 0)
        {
            if ((OutputFormat & LogFormatOptions.ShowLocation) != 0)
            {
                outputBuilder.Append($" [{member} @ {Path.GetFileName(file)}:{line}]");
            }
            else
            {
                outputBuilder.Append($" [{member}]");
            }
        }
        else if ((OutputFormat & LogFormatOptions.ShowLocation) != 0)
        {
            outputBuilder.Append($" [@ {Path.GetFileName(file)}:{line}]");
        }

        WriteLine(outputBuilder.ToString());
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
        Log(LoggerVerbosity.Info, "[INFO] " + message, member, file, line);
    }

    public void LogWarning(
        string message,
        [CallerMemberName] string member = "",
        [CallerFilePath] string file = "",
        [CallerLineNumber] int line = 0)
    {
        Log(LoggerVerbosity.Warning, "[WARN] " + message, member, file, line);
    }

    public void LogCritical(
        string message,
        [CallerMemberName] string member = "",
        [CallerFilePath] string file = "",
        [CallerLineNumber] int line = 0)
    {
        Log(LoggerVerbosity.Critical, "[CRIT] " + message, member, file, line);
    }
}
