using AslHelp.Shared.Results;

namespace AslHelp.Ipc;

public sealed record IpcResponseMessage<TPayload>(
    TPayload? Payload = default,
    string? Error = default)
{
    public static IpcResponseMessage<TPayload> FromResult(IResult<TPayload> result)
    {
        if (result.Value is { } value)
        {
            return new(Payload: value);
        }
        else if (result.Error is { } error)
        {
            return new(Error: error.Message);
        }
        else
        {
            return new(Error: "Invalid result produced by visitor");
        }
    }
}
