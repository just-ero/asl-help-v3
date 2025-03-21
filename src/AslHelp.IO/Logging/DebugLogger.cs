using System.Diagnostics;

namespace AslHelp.IO.Logging;

public sealed class DebugLogger : Logger
{
    protected override void WriteLine(string output)
    {
        Debug.WriteLine(output);
    }
}
