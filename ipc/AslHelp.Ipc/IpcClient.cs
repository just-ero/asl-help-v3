using System;
using System.IO.Pipes;
using System.Text.Json.Serialization;

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

    public void Connect()
    {
        _pipe.Connect();
    }

    protected Result<TResponse> Transmit<TResponse>(TRequestPayloadBase request)
        where TResponse : TResponsePayloadBase
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

    public void Dispose()
    {
        _pipe.Dispose();
    }
}
