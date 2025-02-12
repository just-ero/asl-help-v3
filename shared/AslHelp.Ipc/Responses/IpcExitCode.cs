using System;

using AslHelp.Ipc.Protocol;

namespace AslHelp.Ipc.Responses;

internal enum IpcExitCode : sbyte
{
    UnhandledException = -1,
    UnknownRequest = -2,
    InvalidPacket = -3
}

internal readonly struct IpcExitCodeDescriptor : IExitCodeDescriptor<IpcExitCode>
{
    public IpcExitCode Ok => throw new NotImplementedException();

    public bool IsOk(IpcExitCode exitCode)
    {
        throw new NotImplementedException();
    }

    public string GetMessage(IpcExitCode exitCode)
    {
        return exitCode switch
        {
            IpcExitCode.UnhandledException => "An unhandled exception occurred.",
            IpcExitCode.UnknownRequest => "An unknown request was received.",
            IpcExitCode.InvalidPacket => "An invalid packet was received.",
            _ => $"Unknown exit code: {exitCode}."
        };
    }
}
