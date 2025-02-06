using System;

using AslHelp.Ipc.Protocol;

namespace AslHelp.Ipc.Mono.Api;

public readonly record struct GetMonoImage : IIpcFunc<
    MonoCommand,
    GetMonoImage.ExitCode,
    GetMonoImage.Request,
    GetMonoImage.Response>
{
    public MonoCommand Command => MonoCommand.GetMonoImage;
    public ExitCode OkCode => ExitCode.Ok;

    // Protocols

    public enum ExitCode : sbyte
    {
        Ok,
        NotFound
    }

    public readonly record struct Request(
        string Name) : IRequest;

    public readonly record struct Response(
        ulong Address,
        string Name,
        string ModuleName,
        string FileName) : IResponse;
}
