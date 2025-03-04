namespace AslHelp.Ipc;

public sealed class IpcResponseMessage<TPayload>
{
    public TPayload? Payload { get; }
    public string? Error { get; }
}
