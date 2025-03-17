using System.Runtime.InteropServices;
using System.Threading.Tasks;

using AslHelp.IO.Logging;
using AslHelp.Ipc;
using AslHelp.Native.Mono;

namespace AslHelp.Native;

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

        _serverTask = Task.Run(() =>
        {
            NativeMonoServer server = new(_logger);
            server.Start();
        });

        _logger.LogDetail("IPC server started.");
        return true;
    }
}
