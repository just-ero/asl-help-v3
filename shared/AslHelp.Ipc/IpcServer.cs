using System;
using System.IO.Pipes;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

using AslHelp.Ipc.Serialization;

namespace AslHelp.Ipc;

public abstract class IpcServer<TRequestPayloadBase, TResponsePayloadBase> : IDisposable
{
    private readonly NamedPipeServerStream _pipe;

    public IpcServer(string pipeName)
        : this(pipeName, PipeOptions.None) { }

    public IpcServer(string pipeName, PipeOptions options)
    {
        _pipe = new(pipeName, PipeDirection.InOut, 1, PipeTransmissionMode.Byte, options);
    }

    protected abstract JsonSerializerContext SerializerContext { get; }

    public void WaitForConnection()
    {
        _pipe.WaitForConnection();
    }

    public async Task WaitForConnectionAsync()
    {
        await _pipe.WaitForConnectionAsync();
        Console.WriteLine("Connected.");
    }

    public async Task ProcessMessage()
    {
        Console.WriteLine("Waiting for message...");

        try
        {
            var message = await IpcSerializer.DeserializeAsync<IpcRequestMessage<TRequestPayloadBase>>(_pipe, SerializerContext);
            if (message is null)
            {
                Console.WriteLine("Received null message.");
                return;
            }

            Console.WriteLine($"Received {message.Payload}.");

            HandleMessage(message);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    protected abstract void HandleMessage(IpcRequestMessage<TRequestPayloadBase> message);

    protected async Task SendMessage<TResponse>(IpcRequestMessage<TResponse> message)
        where TResponse : TResponsePayloadBase
    {
        await IpcSerializer.SerializeAsync(_pipe, message, SerializerContext);
    }

    public void Dispose()
    {
        _pipe.Dispose();
    }
}
