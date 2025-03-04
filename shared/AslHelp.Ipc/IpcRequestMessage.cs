namespace AslHelp.Ipc;

public sealed record IpcRequestMessage<TPayload>(
    TPayload Payload);
