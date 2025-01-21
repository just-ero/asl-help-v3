using System;

using AslHelp.Ipc.Errors;
using AslHelp.Ipc.Protocol;
using AslHelp.Shared.Results.Errors;

namespace AslHelp.Ipc.Mono.Api;

public readonly record struct GetMonoImage : IIpcFunc<
    MonoCommand,
    GetMonoImage.ExitCode,
    GetMonoImage.Request,
    GetMonoImage.Response>
{
    public MonoCommand Command => MonoCommand.GetMonoImage;

    public ExitCode OkCode => ExitCode.Ok;

    public enum ExitCode : byte
    {
        Ok,

        [Err("Image '{0}' not found.")]
        NotFound
    }

    public readonly record struct Request(
        string Name) : IRequest<ExitCode>
    {
        public IResultError GetErr(ExitCode code)
        {
            return code switch
            {
                ExitCode.NotFound => $"Image '{Name}' not found.",
                _ => throw new ArgumentOutOfRangeException(nameof(code), code, null)
            };
        }
    }

    public readonly record struct Response(
        ulong Address,
        string Name,
        string ModuleName,
        string FileName) : IResponse;

    public readonly record struct Error(
        string Message) : IResultError;
}
