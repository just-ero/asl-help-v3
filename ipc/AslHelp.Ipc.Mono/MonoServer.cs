using System.IO.Pipes;
using System.Text.Json.Serialization;

using AslHelp.IO.Logging;
using AslHelp.Ipc.Mono.Commands;
using AslHelp.Shared.Results;

namespace AslHelp.Ipc.Mono;

public abstract class MonoServer : IpcServer<IMonoRequest<IMonoResponse>, IMonoResponse>, IMonoVisitor
{
    protected MonoServer(string pipeName, Logger? logger = null)
        : base(pipeName, logger) { }

    protected MonoServer(string pipeName, PipeOptions options, Logger? logger = null)
        : base(pipeName, options, logger) { }

    protected override JsonSerializerContext SerializerContext { get; } = new MonoSerializerContext();

    protected sealed override IResult<IMonoResponse> HandleRequest(IMonoRequest<IMonoResponse> payload)
    {
        return payload.Handle(this);
    }

    protected abstract Result<GetMonoImageResponse> GetMonoImage(GetMonoImageRequest request);
    protected abstract Result<GetMonoClassResponse> GetMonoClass(GetMonoClassRequest request);

    Result<GetMonoImageResponse> IMonoVisitor.GetMonoImage(GetMonoImageRequest request)
    {
        return GetMonoImage(request);
    }

    Result<GetMonoClassResponse> IMonoVisitor.GetMonoClass(GetMonoClassRequest request)
    {
        return GetMonoClass(request);
    }
}
