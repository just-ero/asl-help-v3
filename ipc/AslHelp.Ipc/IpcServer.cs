using System;
using System.IO.Pipes;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

using AslHelp.IO.Logging;
using AslHelp.Ipc.Serialization;
using AslHelp.Shared.Results;

namespace AslHelp.Ipc;

public abstract class IpcServer<TRequestPayloadBase, TResponsePayloadBase> : IDisposable
{
    private readonly string _name;

    private readonly NamedPipeServerStream _pipe;
    private readonly Logger? _logger;

    protected IpcServer(string pipeName, Logger? logger = null)
        : this(pipeName, PipeOptions.None, logger) { }

    protected IpcServer(string pipeName, PipeOptions options, Logger? logger = null)
    {
        _name = GetType().Name;

        _pipe = new(pipeName, PipeDirection.InOut, 1, PipeTransmissionMode.Byte, options);
        _logger = logger;
    }

    protected abstract JsonSerializerContext SerializerContext { get; }

    public void WaitForConnection()
    {
        _logger?.LogDetail($"[{_name}] [WaitForConnection] Waiting for connection...");

        _pipe.WaitForConnection();

        _logger?.LogDetail($"[{_name}] [WaitForConnection] Connected.");
    }

    public async Task WaitForConnectionAsync()
    {
        _logger?.LogDetail($"[{_name}] [WaitForConnectionAsync] Waiting for connection...");

        await _pipe.WaitForConnectionAsync();

        _logger?.LogDetail($"[{_name}] [WaitForConnectionAsync] Connected.");
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
            var result = HandleRequest(payload);
            var resMsg = IpcResponseMessage<TResponsePayloadBase>.FromResult(result);

            IpcSerializer.Serialize(_pipe, resMsg, SerializerContext);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    protected abstract IResult<TResponsePayloadBase> HandleRequest(TRequestPayloadBase request);

    public void Dispose()
    {
        _pipe.Dispose();
    }
}
