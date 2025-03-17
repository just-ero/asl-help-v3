namespace AslHelp.IO.Logging;

public abstract class Logger
{
    public LoggerVerbosity Verbosity { get; set; }

    protected abstract void Log(string message);

    public void Log(LoggerVerbosity verbosity, string message)
    {
        if (verbosity >= Verbosity)
        {
            Log(message);
        }
    }

    public void LogDetail(string message)
    {
        Log(LoggerVerbosity.Detailed, message);
    }

    public void LogInfo(string message)
    {
        Log(LoggerVerbosity.Info, message);
    }

    public void LogWarning(string message)
    {
        Log(LoggerVerbosity.Warn, message);
    }

    public void LogError(string message)
    {
        Log(LoggerVerbosity.Error, message);
    }
}
