using System.IO.Pipes;

using AslHelp.Ipc.Mono.Protocol;
using AslHelp.Ipc.Responses;

namespace AslHelp.Ipc.Mono;

public abstract class MonoServer : Server<MonoCommand>
{
    protected MonoServer(string pipeName)
        : base(pipeName) { }

    protected MonoServer(string pipeName, PipeOptions options)
        : base(pipeName, options) { }

    protected override IIpcResult HandleCommand(MonoCommand command)
    {
        throw new System.NotImplementedException();
    }

    protected abstract IpcResult<GetMonoImageExitCode, GetMonoImageResponse> GetMonoImage(GetMonoImageRequest request);
}
