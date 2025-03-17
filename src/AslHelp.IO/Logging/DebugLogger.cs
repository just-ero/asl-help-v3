using System.Diagnostics;

namespace AslHelp.IO.Logging;

public sealed class DebugLogger : Logger
{
    protected override void Log(string message)
    {
        Debug.WriteLine(message);
    }
}
