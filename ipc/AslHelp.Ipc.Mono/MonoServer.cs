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

    protected sealed override IResult<IMonoResponse> HandleRequest(IMonoRequest<IMonoResponse> payload)
    {
        return payload.Handle(this);
    }

    protected abstract Result<GetMonoImageResponse> GetMonoImage(GetMonoImageRequest request);

    Result<GetMonoImageResponse> IMonoVisitor.GetMonoImage(GetMonoImageRequest request)
    {
        return GetMonoImage(request);
    }
}
