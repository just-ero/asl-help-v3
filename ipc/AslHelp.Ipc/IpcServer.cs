using System;
using System.IO.Pipes;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

using AslHelp.IO.Logging;
using AslHelp.Ipc.Serialization;
using AslHelp.Shared.Results;

namespace AslHelp.Ipc;

public abstract class IpcServer<TRequestPayloadBase, TResponsePayloadBase> : IDisposable
{
    private readonly NamedPipeServerStream _pipe;

    protected IpcServer(string pipeName, Logger? logger = null)
        : this(pipeName, PipeOptions.None, logger) { }

    protected IpcServer(string pipeName, PipeOptions options, Logger? logger = null)
    {
        _pipe = new(pipeName, PipeDirection.InOut, 1, PipeTransmissionMode.Byte, options);
        Logger = logger;
    }

    protected Logger? Logger { get; }

    protected abstract JsonSerializerContext SerializerContext { get; }

    public void Run()
    {
        Logger?.LogDetail("Server started.");

        while (true)
        {
            Logger?.LogDetail("Waiting for connection...");
            _pipe.WaitForConnection();

            Logger?.LogDetail("Connected!");

            while (true)
            {
                try
                {
                    Logger?.LogDetail("Waiting for request...");

                    var req = IpcSerializer.Deserialize<IpcRequestMessage<TRequestPayloadBase>>(_pipe, SerializerContext);
                    if (req is null)
                    {
                        Logger?.LogDetail("  => Received null message.");

                        var res = IpcResponseMessage.NullRequestMessage<TResponsePayloadBase>();
                        IpcSerializer.Serialize(_pipe, res, SerializerContext);
                    }
                    else if (req.Data is null)
                    {
                        Logger?.LogDetail("  => Received message with null payload.");

                        var res = IpcResponseMessage.NullPayloadMessage<TResponsePayloadBase>();
                        IpcSerializer.Serialize(_pipe, res, SerializerContext);
                    }
                    else
                    {
                        Logger?.LogDetail($"  => Received message with data: {req.Data}.");

                        var res = IpcResponseMessage.FromResult(HandleRequest(req.Data));
                        IpcSerializer.Serialize(_pipe, res, SerializerContext);
                    }
                }
                catch (Exception ex)
                {
                    Logger?.LogDetail($"  => An error occurred while processing the request.");
                    Logger?.LogDetail($"    => {ex.Message}");

                    var res = IpcResponseMessage.FromException<TResponsePayloadBase>(ex);
                    IpcSerializer.Serialize(_pipe, res, SerializerContext);
                }
            }
        }
    }

    public async Task RunAsync(CancellationToken ct = default)
    {
        Logger?.LogDetail("Server started.");

        while (true)
        {
            Logger?.LogDetail("Waiting for connection...");
            await _pipe.WaitForConnectionAsync(ct);

            Logger?.LogDetail("Connected!");

            while (true)
            {
                try
                {
                    Logger?.LogDetail("Waiting for request...");

                    var req = await IpcSerializer.DeserializeAsync<IpcRequestMessage<TRequestPayloadBase>>(_pipe, SerializerContext, ct);
                    if (req is null)
                    {
                        Logger?.LogDetail("  => Received null message.");

                        var res = IpcResponseMessage.NullRequestMessage<TResponsePayloadBase>();
                        await IpcSerializer.SerializeAsync(_pipe, res, SerializerContext, ct);
                    }
                    else if (req.Data is null)
                    {
                        Logger?.LogDetail("  => Received message with null payload.");

                        var res = IpcResponseMessage.NullPayloadMessage<TResponsePayloadBase>();
                        await IpcSerializer.SerializeAsync(_pipe, res, SerializerContext, ct);
                    }
                    else
                    {
                        Logger?.LogDetail($"  => Received message with data: {req.Data}.");

                        var res = IpcResponseMessage.FromResult(HandleRequest(req.Data));
                        await IpcSerializer.SerializeAsync(_pipe, res, SerializerContext, ct);
                    }
                }
                catch (Exception ex)
                {
                    Logger?.LogDetail($"  => An error occurred while processing the request.");
                    Logger?.LogDetail($"    => {ex.Message}");

                    var res = IpcResponseMessage.FromException<TResponsePayloadBase>(ex);
                    await IpcSerializer.SerializeAsync(_pipe, res, SerializerContext, ct);
                }
            }
        }
    }

    protected abstract IResult<TResponsePayloadBase> HandleRequest(TRequestPayloadBase request);

    public void Dispose()
    {
        _pipe.Dispose();
    }
}
