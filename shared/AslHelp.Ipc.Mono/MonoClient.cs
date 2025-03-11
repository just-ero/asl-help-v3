using System.IO.Pipes;
using System.Text.Json.Serialization;

using AslHelp.Ipc.Mono.Commands;
using AslHelp.Shared.Results;

namespace AslHelp.Ipc.Mono;

public sealed class MonoClient : IpcClient<IMonoRequest<IMonoResponse>, IMonoResponse>, IMonoVisitor
{
    public MonoClient(string pipeName)
        : base(pipeName) { }

    public MonoClient(string pipeName, PipeOptions options)
        : base(pipeName, options) { }

    protected override JsonSerializerContext SerializerContext { get; } = new MonoSerializerContext();

    public Result<GetMonoImageResponse> GetMonoImage(GetMonoImageRequest request)
    {
        return Transmit<GetMonoImageResponse>(request);
    }
}
