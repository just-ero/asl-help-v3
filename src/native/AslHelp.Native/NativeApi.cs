using System;
using System.Runtime.InteropServices;

using AslHelp.IO.Logging;
using AslHelp.Ipc.Native.Mono;
using AslHelp.Ipc.Native.Unity;
using AslHelp.Native.Interop;
using AslHelp.Native.Interop.Commands;

namespace AslHelp.Ipc.Native;

internal sealed unsafe partial class NativeApi
{
    private static readonly DebugLogger _logger = new();

    private static NativeMonoServer? _monoServer;

    [UnmanagedCallersOnly(EntryPoint = Cmd.StartServer)]
    public static uint StartServer(StartServerRequest* request)
    {
        _logger.LogDetail($"{Cmd.StartServer}({*request})");

        try
        {
            switch (*request)
            {
                case StartServerRequest.StartMonoServer:
                {
                    return (uint)StartMono();
                }
                default:
                {
                    _logger.LogCritical($"Unknown request: {*request}");
                    return (uint)StartServerResponse.RequestUnknown;
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogCritical($"Unhandled exception: {ex}");
            return (uint)StartServerResponse.UnhandledException;
        }
    }

    private static StartServerResponse StartMono()
    {
        if (_monoServer is { IsRunning: true })
        {
            _logger.LogDetail("Server already running.");
            return StartServerResponse.Ok;
        }

        _logger.LogDetail("Searching for compatible library...");

        if (NativeLibrary.TryLoad("mono.dll", out nint hModule))
        {
            _logger.LogDetail("  => Found 'mono.dll'.");

            MonoApi api = new(hModule);
            _monoServer = new(api, _logger);
            _ = _monoServer.RunAsync();

            return StartServerResponse.Ok;
        }
        else if (NativeLibrary.TryLoad("mono-2.0-bdwgc.dll", out hModule))
        {
            _logger.LogDetail("  => Found 'mono-2.0-bdwgc.dll'.");

            MonoApi api = new(hModule);
            _monoServer = new(api, _logger);
            _ = _monoServer.RunAsync();

            return StartServerResponse.Ok;
        }
        else
        {
            _logger.LogCritical("Failed to load 'mono' library.");
            return StartServerResponse.LibraryNotFound;
        }
    }
}
