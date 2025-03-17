using System;

namespace AslHelp.IO.Logging;

public sealed class ConsoleLogger : Logger
{
    protected override void WriteLine(string output)
    {
        Console.WriteLine(output);
    }
}
