using System.Runtime.InteropServices;

using AslHelp.Engines;
using AslHelp.IO.Logging;
using AslHelp.Ipc.Native.Mono;

namespace AslHelp.Ipc.Native;

internal static partial class Exports
{
    private static readonly DebugLogger _logger = new();
    private static readonly NativeMonoServer _monoServer = new(_logger);

    [UnmanagedCallersOnly(EntryPoint = IpcConnection.EntryPoint)]
    public static unsafe bool StartServer(Engine* gameEngine)
    {
        switch (*gameEngine)
        {
            case Engine.Mono:
            {
                if (_monoServer.IsRunning)
                {
                    return true;
                }

                _logger.LogDetail("Starting Mono server.");
                _ = _monoServer.RunAsync();

                return true;
            }
            default:
                _logger.LogCritical("Unknown engine.");
                return false;
        }
    }
}
