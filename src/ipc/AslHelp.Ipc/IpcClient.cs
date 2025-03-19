using System;
using System.IO;
using System.IO.Pipes;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

using AslHelp.Ipc.Errors;
using AslHelp.Ipc.Serialization;
using AslHelp.Shared.Results;

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

    public bool IsConnected => _pipe.IsConnected;

    public void Connect(int timeout = -1)
    {
        _pipe.Connect(timeout);
    }

    public async Task ConnectAsync(CancellationToken ct = default)
    {
        await _pipe.ConnectAsync(ct);
    }

    public async Task ConnectAsync(int timeout, CancellationToken ct = default)
    {
        await _pipe.ConnectAsync(timeout, ct);
    }

    protected Result<TResponse> Transmit<TResponse>(TRequestPayloadBase request)
        where TResponse : TResponsePayloadBase
    {
        try
        {
            IpcRequestMessage<TRequestPayloadBase> requestMessage = new(request);
            IpcSerializer.Serialize(_pipe, requestMessage, SerializerContext);

            var res = IpcSerializer.Deserialize<IpcResponseMessage<TResponsePayloadBase>>(_pipe, SerializerContext);
            if (res is null)
            {
                return IpcError.NullResponse;
            }
            else if (res.Data is TResponse data)
            {
                return data;
            }
            else if (res.Error is not null)
            {
                return IpcError.Other(res.Error);
            }
            else
            {
                return IpcError.NullResponse;
            }
        }
        catch (IOException) when (!IsConnected)
        {
            return IpcError.ConnectionClosedByServer;
        }
    }

    protected async Task<Result<TResponse>> TransmitAsync<TResponse>(TRequestPayloadBase request, CancellationToken ct = default)
        where TResponse : TResponsePayloadBase
    {
        try
        {
            IpcRequestMessage<TRequestPayloadBase> requestMessage = new(request);
            await IpcSerializer.SerializeAsync(_pipe, requestMessage, SerializerContext, ct);

            var res = await IpcSerializer.DeserializeAsync<IpcResponseMessage<TResponsePayloadBase>>(_pipe, SerializerContext, ct);
            if (res is null)
            {
                return IpcError.NullResponse;
            }
            else if (res.Data is TResponse data)
            {
                return data;
            }
            else if (res.Error is not null)
            {
                return IpcError.Other(res.Error);
            }
            else
            {
                return IpcError.NullResponse;
            }
        }
        catch (IOException) when (!IsConnected)
        {
            return IpcError.ConnectionClosedByServer;
        }
    }

    public void Dispose()
    {
        _pipe.Dispose();
    }
}
