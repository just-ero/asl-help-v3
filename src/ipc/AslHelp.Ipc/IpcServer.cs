using System;
using System.IO;
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

    public bool IsConnected => _pipe.IsConnected;
    public bool IsRunning { get; private set; }

    protected Logger? Logger { get; }

    protected abstract JsonSerializerContext SerializerContext { get; }

    private async Task HandleRequestAsync(CancellationToken ct)
    {
        Logger?.LogDetail("Waiting for next request...");

        try
        {
            var req = await IpcSerializer.DeserializeAsync<IpcRequestMessage<TRequestPayloadBase>>(_pipe, SerializerContext, ct);
            if (req is { Data: { } data })
            {
                Logger?.LogDetail($"  => {data}");

                var res = IpcResponseMessage.FromResult(HandleRequest(data));
                await IpcSerializer.SerializeAsync(_pipe, res, SerializerContext, ct);
            }
            else
            {
                Logger?.LogDetail($"  => `null`");

                var res = IpcResponseMessage.NullRequestMessage<TResponsePayloadBase>();
                await IpcSerializer.SerializeAsync(_pipe, res, SerializerContext, ct);
            }
        }
        catch (EndOfStreamException) when (!IsConnected)
        {
            Logger?.LogDetail("  => Connection closed by client.");
        }
        catch (Exception ex)
        {
            Logger?.LogInfo($"  => Internal server error: {ex}");

            if (IsConnected)
            {
                var res = IpcResponseMessage.FromException<TResponsePayloadBase>(ex);
                await IpcSerializer.SerializeAsync(_pipe, res, SerializerContext, ct);
            }
        }
    }

    public async Task RunAsync(CancellationToken ct = default)
    {
        if (IsRunning)
        {
            return;
        }

        IsRunning = true;

        while (!ct.IsCancellationRequested)
        {
            Logger?.LogDetail("Waiting for connection...");
            await _pipe.WaitForConnectionAsync(ct);

            Logger?.LogDetail("  => Connected!");

            while (IsConnected)
            {
                await HandleRequestAsync(ct);
            }

            Logger?.LogDetail("Connection closed.");
            _pipe.Disconnect();
        }

        IsRunning = false;
    }

    protected abstract IResult<TResponsePayloadBase> HandleRequest(TRequestPayloadBase request);

    public void Dispose()
    {
        _pipe.Dispose();
    }
}
