using System;

namespace AslHelp.Interop.Commands;

public interface IRemoteCommand<out TResult>
    where TResult : unmanaged, Enum
{
    string Uid { get; }
}

public interface IRemoteCommand<in TRequest, out TResult>
    where TRequest : unmanaged
    where TResult : unmanaged, Enum
{
    string Uid { get; }
}
