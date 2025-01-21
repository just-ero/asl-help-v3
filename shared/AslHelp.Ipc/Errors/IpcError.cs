using System;
using System.Reflection;

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

    public static IpcError FromStatus<TStatus>(TStatus status)
        where TStatus : struct, Enum
    {
        string fullName = status.ToString();
        string message = typeof(TStatus).GetField(fullName)?.GetCustomAttribute<ErrAttribute>() is { } attr
            ? $"{fullName}: {attr.Message}"
            : fullName;

        return new(message);
    }

    public static IpcError FromException(Exception exception)
    {
        return new(exception.Message);
    }
}
