using System;

namespace AslHelp.Ipc.Protocol;

public interface IIpcEndpoint<out TCommand, out TExitCode>
    where TCommand : unmanaged, Enum
    where TExitCode : unmanaged, Enum
{
    TCommand Command { get; }
    TExitCode OkCode { get; }
}

public interface IIpcAction<out TCommand, out TExitCode> : IIpcEndpoint<TCommand, TExitCode>
    where TCommand : unmanaged, Enum
    where TExitCode : unmanaged, Enum;

public interface IIpcAction<out TCommand, out TExitCode, out TRequest> : IIpcEndpoint<TCommand, TExitCode>
    where TCommand : unmanaged, Enum
    where TExitCode : unmanaged, Enum
    where TRequest : IRequest<TExitCode>;

public interface IIpcFunc<out TCommand, out TExitCode, out TResponse> : IIpcEndpoint<TCommand, TExitCode>
    where TCommand : unmanaged, Enum
    where TExitCode : unmanaged, Enum
    where TResponse : IResponse;

public interface IIpcFunc<out TCommand, out TExitCode, out TRequest, out TResponse> : IIpcEndpoint<TCommand, TExitCode>
    where TCommand : unmanaged, Enum
    where TExitCode : unmanaged, Enum
    where TRequest : IRequest<TExitCode>
    where TResponse : IResponse;
