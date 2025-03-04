using System;
using System.IO.Pipes;
using System.Text.Json;
using System.Text.Json.Serialization;

using AslHelp.Ipc.Serialization;

namespace AslHelp.Ipc;

public abstract class IpcClient<TRequestPayloadBase, TResponsePayloadBase> : IDisposable
{
    private readonly NamedPipeClientStream _pipe;

    public IpcClient(string pipeName)
        : this(pipeName, PipeOptions.None) { }

    public IpcClient(string pipeName, PipeOptions options)
    {
        _pipe = new(".", pipeName, PipeDirection.InOut, options);
    }

    protected abstract JsonSerializerContext SerializerContext { get; }

    public void Connect()
    {
        Console.WriteLine("Connecting...");
        _pipe.Connect();
    }

    protected void SendMessage(IpcRequestMessage<TRequestPayloadBase> message)
    {
        Console.WriteLine("Sending message...");
        IpcSerializer.Serialize(_pipe, message, SerializerContext);

        Console.WriteLine("Message sent.");
    }

    protected IpcRequestMessage<TResponse>? ReceiveMessage<TResponse>()
        where TResponse : TResponsePayloadBase
    {
        return (IpcRequestMessage<TResponse>?)JsonSerializer.Deserialize(_pipe, typeof(IpcRequestMessage<TResponse>), SerializerContext);
    }

    public void Dispose()
    {
        _pipe.Dispose();
    }
}
