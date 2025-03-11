using System.Text.Json.Serialization;

using AslHelp.Ipc.Mono.Transmission;
using AslHelp.Ipc.Mono.Transmission.Commands;
using AslHelp.Shared.Results;

namespace AslHelp.Ipc.Mono;

public abstract class MonoServer : IpcServer<IMonoRequest<IMonoResponse>, IMonoResponse>, IMonoVisitor
{
    public MonoServer(string pipeName)
        : base(pipeName) { }

    protected override JsonSerializerContext SerializerContext { get; } = new MonoSerializerContext();

    protected sealed override IResult<IMonoResponse> HandleMessage(IMonoRequest<IMonoResponse> payload)
    {
        return payload.Visit(this);
    }

    public abstract Result<GetMonoImageResponse> GetMonoImage(GetMonoImageRequest request);
}
