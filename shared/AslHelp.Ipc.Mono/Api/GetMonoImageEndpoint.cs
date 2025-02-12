using AslHelp.Ipc.Mono.Protocol;
using AslHelp.Ipc.Protocol;

namespace AslHelp.Ipc.Mono.Api;

public readonly record struct GetMonoImageEndpoint : IIpcFunc<
    MonoCommand,
    GetMonoImageExitCode,
    GetMonoImageRequest,
    GetMonoImageResponse>
{
    public MonoCommand Command => MonoCommand.GetMonoImage;
    public GetMonoImageExitCode OkCode => GetMonoImageExitCode.Ok;
}
