using System;
using System.Runtime.InteropServices;

using AslHelp.IO.Logging;
using AslHelp.Ipc.Mono;

namespace AslHelp.Interop.Native;

public static class NativeCommandExports
{
    private static readonly DebugLogger _logger = new()
    {
        OutputFormat = LogFormatOptions.ShowMember | LogFormatOptions.ShowLocation,
        Verbosity = LoggerVerbosity.Detailed
    };

    private static MonoServer? _monoServer;

    [UnmanagedCallersOnly(EntryPoint = Commands.StartServer.Uid)]
    public static unsafe Commands.StartServer.ExitCode StartServer(Commands.StartServer.Command* command)
    {
        _logger.LogDetail($"Received '{*command}' for {Commands.StartServer.Uid}.");

        try
        {
            switch (*command)
            {
                case Commands.StartServer.Command.StartMonoServer:
                {
                    return StartMonoServer();
                }
                default:
                {
                    _logger.LogCritical($"Unknown argument '{*command}' for {Commands.StartServer.Uid}.");
                    return Commands.StartServer.ExitCode.RequestUnknown;
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogCritical($"Unhandled exception: {ex}");
            return Commands.StartServer.ExitCode.UnhandledException;
        }
    }

    private static Commands.StartServer.ExitCode StartMono()
    {
        if (_monoServer is { IsRunning: true })
        {
            _logger.LogDetail("Server already running.");
            return Commands.StartServer.ExitCode.Ok;
        }

        _logger.LogDetail("Searching for compatible library...");

        if (NativeLibrary.TryLoad("mono.dll", out nint hModule))
        {
            _logger.LogDetail("  => Found 'mono.dll'.");

            MonoApi api = new(hModule);
            _monoServer = new(api, _logger);
            _ = _monoServer.RunAsync();

            return Commands.StartServer.ExitCode.Ok;
        }
        else if (NativeLibrary.TryLoad("mono-2.0-bdwgc.dll", out hModule))
        {
            _logger.LogDetail("  => Found 'mono-2.0-bdwgc.dll'.");

            MonoApi api = new(hModule);
            _monoServer = new(api, _logger);
            _ = _monoServer.RunAsync();

            return Commands.StartServer.ExitCode.Ok;
        }
        else
        {
            _logger.LogCritical("Failed to load 'mono' library.");
            return Commands.StartServer.ExitCode.MonoLibraryNotFound;
        }
    }
}
