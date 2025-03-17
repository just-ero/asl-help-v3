using System.Runtime.InteropServices;
using System.Threading.Tasks;

using AslHelp.IO.Logging;
using AslHelp.Ipc.Native.Mono;

namespace AslHelp.Ipc.Native;

internal static partial class Exports
{
    private static DebugLogger? _logger;
    private static Task? _serverTask;

    [UnmanagedCallersOnly(EntryPoint = IpcConnection.EntryPoint)]
    public static unsafe bool EntryPoint(int connectionType)
    {
        if (_logger is not null)
        {
            _logger.LogDetail("IPC server already running.");
            return false;
        }

        _logger = new();
        _logger.LogDetail("Starting IPC server...");

        NativeMonoServer server = new(_logger);
        _serverTask = server.RunAsync();

        _logger.LogDetail("IPC server started.");
        return true;
    }
}
