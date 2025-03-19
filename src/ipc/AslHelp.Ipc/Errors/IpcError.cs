using AslHelp.Shared.Results.Errors;

namespace AslHelp.Ipc.Errors;

internal sealed record IpcError : ResultError
{
    private IpcError(string message)
        : base(message) { }

    public static IpcError Other(string message)
    {
        return new(message);
    }

    public static IpcError ConnectionClosedByServer => new("Connection closed by server.");
    public static IpcError NullResponse => new("Received null response from server.");
}
