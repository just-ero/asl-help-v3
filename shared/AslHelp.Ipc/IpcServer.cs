using System;
using System.IO.Pipes;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

using AslHelp.Ipc.Serialization;
using AslHelp.Shared.Results;

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

        var reqMsg = await IpcSerializer.DeserializeAsync<IpcRequestMessage<TRequestPayloadBase>>(_pipe, SerializerContext);
        if (reqMsg is not { Payload: { } payload })
        {
            Console.WriteLine("Received null message.");
            return;
        }

        Console.WriteLine($"Received {payload}.");

        try
        {
            var result = HandleMessage(payload);
            var resMsg = IpcResponseMessage<TResponsePayloadBase>.FromResult(result);

            IpcSerializer.Serialize(_pipe, resMsg, SerializerContext);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    protected abstract IResult<TResponsePayloadBase> HandleMessage(TRequestPayloadBase payload);

    public void Dispose()
    {
        _pipe.Dispose();
    }
}
