using System.IO.Pipes;
using System.Text.Json.Serialization;

using AslHelp.Ipc.Mono.Requests;
using AslHelp.Ipc.Mono.Responses;

namespace AslHelp.Ipc.Mono;

public sealed class MonoClient : IpcClient<MonoRequestBase, MonoResponseBase>
{
    public MonoClient(string pipeName)
        : base(pipeName) { }

    public MonoClient(string pipeName, PipeOptions options)
        : base(pipeName, options) { }

    protected override JsonSerializerContext SerializerContext { get; } = new MonoSerializerContext();

    public void GetMonoImage(string name)
    {
        SendMessage(new(new GetMonoImageRequest(name)));
    }
}
