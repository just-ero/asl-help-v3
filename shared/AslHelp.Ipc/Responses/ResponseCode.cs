using AslHelp.Ipc.Errors;

namespace AslHelp.Ipc.Responses;

public enum IpcExitCode : sbyte
{
    [Err("Unhandled exception occurred.")]
    UnhandledException = -1,

    [Err("Request code was not known by the server.")]
    UnknownRequest = -2,

    [Err("Packet was null or otherwise invalid.")]
    InvalidPacket = -3
}
