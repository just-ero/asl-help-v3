using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace AslHelp.Native;

internal static class Trace
{
    [Conditional("DEBUG")]
    [OverloadResolutionPriority(1)]
    public static void Log(object? output)
    {
        Debug.WriteLine($"[asl-help] {output}");
    }

    [Conditional("DEBUG")]
    public static void Log(string format, params object?[] args)
    {
        Log(string.Format(format, args));
    }
}
