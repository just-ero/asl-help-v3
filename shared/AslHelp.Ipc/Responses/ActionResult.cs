using System;

using AslHelp.Ipc.Protocol;

namespace AslHelp.Ipc.Responses;

public interface IActionResult
{
    string? Message { get; }
    IResponse? Value { get; }
}

public interface IActionResult<out TExitCode> : IActionResult
    where TExitCode : struct, Enum
{
    new TExitCode Code { get; }
}

public interface IActionResult<out TExitCode, out TResponse> : IActionResult<TExitCode>
    where TExitCode : struct, Enum
    where TResponse : IResponse
{
    new TResponse? Response { get; }
}

public readonly struct ActionResult<TExitCode> : IActionResult<TExitCode>
    where TExitCode : struct, Enum
{
    public ActionResult(TExitCode code)
    {
        Code = code;
    }

    public TExitCode Code { get; }
    public IResponse? Value => default;

    public static implicit operator ActionResult<TExitCode>(TExitCode code)
    {
        return new(code);
    }

    Enum IActionResult.Code => Code;
}

public readonly struct ActionResult<TExitCode, TResponse> : IActionResult<TExitCode, TResponse>
    where TExitCode : struct, Enum
    where TResponse : IResponse
{
    public ActionResult(TExitCode code)
    {
        Code = code;
    }

    public ActionResult(TExitCode ok, TResponse response)
    {
        Code = ok;
        Response = response;
    }

    public TExitCode Code { get; }
    public TResponse? Response { get; }

    public static implicit operator ActionResult<TExitCode, TResponse>(TExitCode code)
    {
        return new(code);
    }

    Enum IActionResult.Code => Code;
    IResponse? IActionResult.Value => Response;
}
