using AslHelp.Shared.Results.Errors;

namespace AslHelp.Ipc.Protocol;

public sealed record IpcError(string Message) : ResultError(Message);
