using System.IO.Pipes;
using System.Runtime.CompilerServices;

using AslHelp.Ipc.Mono.Api;
using AslHelp.Shared.Results;

namespace AslHelp.Ipc.Mono;

public sealed class MonoClient : Client<MonoCommand>
{
    public MonoClient(string pipeName)
        : base(pipeName) { }

    public MonoClient(string pipeName, PipeOptions options)
        : base(pipeName, options) { }

    public Result<GetMonoImage.Response> GetMonoImage(string name)
    {
        Unsafe.SkipInit(out GetMonoImage endpoint);
        return CallEndpoint(endpoint,
            request: new(name));
    }
}
