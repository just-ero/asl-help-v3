namespace AslHelp.Ipc;

public sealed record IpcRequestMessage<T>(
    T Data);
