using System;
using System.Text.Json.Serialization;

using AslHelp.Ipc.Mono.Requests;
using AslHelp.Ipc.Mono.Responses;

namespace AslHelp.Ipc.Mono;

public abstract class MonoServer : IpcServer<MonoRequestBase, MonoResponseBase>, IMonoRequestVisitor
{
    public MonoServer(string pipeName)
        : base(pipeName) { }

    protected override JsonSerializerContext SerializerContext { get; } = new MonoSerializerContext();

    protected sealed override void HandleMessage(IpcRequestMessage<MonoRequestBase> message)
    {
        Console.WriteLine("Handling message...");
        message.Payload.Visit(this);
    }

    public abstract GetMonoImageResponse Handle(GetMonoImageRequest request);
}
