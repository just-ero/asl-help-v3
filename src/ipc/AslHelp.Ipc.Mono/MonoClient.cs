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

    public Result<GetMonoImageResponse> GetMonoImage(string name)
    {
        return Transmit<GetMonoImageResponse>(new GetMonoImageRequest(name));
    }

    public Result<GetMonoClassResponse> GetMonoClass(long image, string @namespace, string name)
    {
        return Transmit<GetMonoClassResponse>(new GetMonoClassRequest(image, @namespace, name));
    }

    public Result<GetMonoFieldResponse> GetMonoField(long klass, string name)
    {
        return Transmit<GetMonoFieldResponse>(new GetMonoFieldRequest(klass, name));
    }

    Result<GetMonoClassResponse> IMonoVisitor.GetMonoClass(GetMonoClassRequest request)
    {
        return Transmit<GetMonoClassResponse>(request);
    }

    Result<GetMonoImageResponse> IMonoVisitor.GetMonoImage(GetMonoImageRequest request)
    {
        return Transmit<GetMonoImageResponse>(request);
    }

    Result<GetMonoFieldResponse> IMonoVisitor.GetMonoField(GetMonoFieldRequest request)
    {
        return Transmit<GetMonoFieldResponse>(request);
    }
}
