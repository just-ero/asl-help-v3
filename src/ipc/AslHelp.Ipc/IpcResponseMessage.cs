using System;

using AslHelp.Shared.Results;

namespace AslHelp.Ipc;

public sealed record IpcResponseMessage
{
    public static IpcResponseMessage<T> FromResult<T>(IResult<T> result)
    {
        if (result.Value is { } value)
        {
            return new(Data: value);
        }
        else if (result.Error is { } error)
        {
            return new(Error: error.ToString());
        }
        else
        {
            return new(Error: "Invalid result produced by visitor");
        }
    }

    public static IpcResponseMessage<T> FromException<T>(Exception ex)
    {
        return new(Error: ex.Message);
    }

    public static IpcResponseMessage<T> NullRequestMessage<T>()
    {
        return new(Error: "Received null request.");
    }
}

public sealed record IpcResponseMessage<T>(
    T? Data = default,
    string? Error = default);
