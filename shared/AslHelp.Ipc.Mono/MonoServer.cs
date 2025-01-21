using System.IO.Pipes;
using System.Runtime.CompilerServices;

using AslHelp.Ipc.Mono.Api;
using AslHelp.Ipc.Responses;

namespace AslHelp.Ipc.Mono;

public abstract class MonoServer : Server<MonoCommand>
{
    protected MonoServer(string pipeName)
        : base(pipeName) { }

    protected MonoServer(string pipeName, PipeOptions options)
        : base(pipeName, options) { }

    [SkipLocalsInit]
    protected sealed override void HandleCommand(MonoCommand command)
    {
        switch (command)
        {
            case MonoCommand.GetMonoImage:
            {
                Handle<GetMonoImage.ExitCode, GetMonoImage.Request, GetMonoImage.Response>(
                    GetMonoImage,
                    Api.GetMonoImage.ExitCode.Ok);
                break;
            }
            default:
            {
                Unknown();
                break;
            }
        }
    }

    protected abstract ActionResult<GetMonoImage.ExitCode, GetMonoImage.Response> GetMonoImage(GetMonoImage.Request request);
}
