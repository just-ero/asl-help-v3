using System;

using AslHelp.Ipc.Protocol;

namespace AslHelp.Ipc.Responses;

public interface IIpcResult<out TExitCode>
    where TExitCode : struct, Enum
{
    TExitCode Code { get; }
}

public interface IIpcResult<out TExitCode, out TResponse> : IIpcResult<TExitCode>
    where TExitCode : struct, Enum
    where TResponse : IResponse
{
    TResponse? Response { get; }
}

public readonly struct IpcResult<TExitCode> : IIpcResult<TExitCode>
    where TExitCode : unmanaged, Enum
{
    public IpcResult(TExitCode code)
    {
        Code = code;
    }

    public TExitCode Code { get; }
    public IResponse? Value => default;

    public static implicit operator IpcResult<TExitCode>(TExitCode code)
    {
        return new(code);
    }
}

public readonly struct IpcResult<TExitCode, TResponse> : IIpcResult<TExitCode, TResponse>
    where TExitCode : unmanaged, Enum
    where TResponse : IResponse
{
    public IpcResult(TExitCode code)
    {
        Code = code;
    }

    public IpcResult(TExitCode ok, TResponse response)
    {
        Code = ok;
        Response = response;
    }

    public TExitCode Code { get; }
    public TResponse? Response { get; }

    public static implicit operator IpcResult<TExitCode, TResponse>(TExitCode code)
    {
        return new(code);
    }

    public static implicit operator IpcResult<TExitCode, TResponse>(TResponse response)
    {
        return new(IpcExit<TExitCode>.Ok, response);
    }
}
