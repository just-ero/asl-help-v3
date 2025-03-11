using System.IO.Pipes;
using System.Text.Json.Serialization;

using AslHelp.Ipc.Mono.Commands;
using AslHelp.Shared.Results;

namespace AslHelp.Ipc.Mono;

public abstract class MonoServer : IpcServer<IMonoRequest<IMonoResponse>, IMonoResponse>, IMonoVisitor
{
    protected MonoServer(string pipeName)
        : base(pipeName) { }

    protected MonoServer(string pipeName, PipeOptions options)
        : base(pipeName, options) { }

    protected override JsonSerializerContext SerializerContext { get; } = new MonoSerializerContext();

    protected sealed override IResult<IMonoResponse> HandleMessage(IMonoRequest<IMonoResponse> payload)
    {
        return payload.Visit(this);
    }

    public abstract Result<GetMonoImageResponse> GetMonoImage(GetMonoImageRequest request);
}
