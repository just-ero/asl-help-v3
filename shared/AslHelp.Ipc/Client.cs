using System;
using System.IO.Pipes;

using AslHelp.Ipc.Errors;
using AslHelp.Ipc.Protocol;
using AslHelp.Ipc.Responses;
using AslHelp.Shared.Results;

namespace AslHelp.Ipc;

public class Client<TCommand> : IDisposable
    where TCommand : unmanaged, Enum
{
    private readonly NamedPipeClientStream _pipe;

    public Client(string pipeName)
        : this(pipeName, PipeOptions.None) { }

    public Client(string pipeName, PipeOptions options)
    {
        _pipe = new(".", pipeName, PipeDirection.InOut, options);
    }

    public Result Call<TExitCode>(TCommand command)
        where TExitCode : unmanaged, Enum
    {
        IpcSerializer.Serialize(_pipe, command);

        TExitCode code = IpcSerializer.Deserialize<TExitCode>(_pipe);
        if (!IpcExit<TExitCode>.IsOk(code))
        {
            if (Enum.IsDefined(typeof(IpcExitCode), code))
            {
                return IpcError.FromStatus((IpcExitCode)(Enum)code);
            }

            return IpcError.FromStatus(code);
        }

        return Result.Ok();
    }

    public Result<TResponse> Call<TExitCode, TResponse>(TCommand command, TExitCode ok)
        where TExitCode : unmanaged, Enum
        where TResponse : IResponse
    {
        IpcSerializer.Serialize(_pipe, command);

        TExitCode code = IpcSerializer.Deserialize<TExitCode>(_pipe);
        if (!code.Equals(ok))
        {
            if (Enum.IsDefined(typeof(IpcExitCode), code))
            {
                return IpcError.FromStatus((IpcExitCode)(Enum)code);
            }

            return IpcError.FromStatus(code);
        }

        TResponse? response = IpcSerializer.Deserialize<TResponse?>(_pipe);
        return response is not null
            ? response
            : IpcError.FromStatus(IpcExitCode.InvalidPacket);
    }

    public Result Call<TExitCode, TRequest>(TCommand command, TRequest request, TExitCode ok)
        where TExitCode : unmanaged, Enum
        where TRequest : IRequest
    {
        IpcSerializer.Serialize(_pipe, command);
        IpcSerializer.Serialize(_pipe, request);

        TExitCode code = IpcSerializer.Deserialize<TExitCode>(_pipe);
        if (!code.Equals(ok))
        {
            if (Enum.IsDefined(typeof(IpcExitCode), code))
            {
                return IpcError.FromStatus((IpcExitCode)(Enum)code);
            }

            return IpcError.FromStatus(code);
        }

        return Result.Ok();
    }

    public Result<TResponse> Call<TExitCode, TRequest, TResponse>(TCommand command, TRequest request, TExitCode ok)
        where TExitCode : unmanaged, Enum
        where TRequest : IRequest
        where TResponse : IResponse
    {
        IpcSerializer.Serialize(_pipe, command);
        IpcSerializer.Serialize(_pipe, request);

        TExitCode code = IpcSerializer.Deserialize<TExitCode>(_pipe);
        if (!code.Equals(ok))
        {
            if (Enum.IsDefined(typeof(IpcExitCode), code))
            {
                return IpcError.FromStatus((IpcExitCode)(Enum)code);
            }

            return IpcError.FromStatus(code);
        }

        TResponse? response = IpcSerializer.Deserialize<TResponse?>(_pipe);
        return response is not null
            ? response
            : IpcError.FromStatus<IpcExitCode>(IpcExitCode.InvalidPacket);
    }

    public Result CallEndpoint<TExitCode>(IIpcAction<TCommand, TExitCode> endpoint)
        where TExitCode : unmanaged, Enum
    {
        return Call(
            endpoint.Command,
            endpoint.OkCode);
    }

    public Result CallEndpoint<TExitCode, TRequest>(
        IIpcAction<TCommand, TExitCode, TRequest> endpoint,
        TRequest request)
        where TExitCode : unmanaged, Enum
        where TRequest : IRequest
    {
        return Call(
            endpoint.Command,
            request,
            endpoint.OkCode);
    }

    public Result<TResponse> CallEndpoint<TExitCode, TResponse>(
        IIpcFunc<TCommand, TExitCode, TResponse> endpoint)
        where TExitCode : unmanaged, Enum
        where TResponse : IResponse
    {
        return Call<TExitCode, TResponse>(
            endpoint.Command,
            endpoint.OkCode);
    }

    public Result<TResponse> CallEndpoint<TExitCode, TRequest, TResponse>(
        IIpcFunc<TCommand, TExitCode, TRequest, TResponse> endpoint,
        TRequest request)
        where TExitCode : unmanaged, Enum
        where TRequest : IRequest
        where TResponse : IResponse
    {
        return Call<TExitCode, TRequest, TResponse>(
            endpoint.Command,
            request,
            endpoint.OkCode);
    }

    public void Dispose()
    {
        _pipe.Dispose();
    }
}
