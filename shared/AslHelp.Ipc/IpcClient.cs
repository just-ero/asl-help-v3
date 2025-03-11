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

        var responseMessage = IpcSerializer.Deserialize<IpcResponseMessage<TResponsePayloadBase>>(_pipe, SerializerContext);
        if (responseMessage is null)
        {
            return IpcError.NullResponse;
        }

        if (responseMessage.Error is not null)
        {
            return IpcError.Other(responseMessage.Error);
        }

        if (responseMessage.Payload is not TResponse response)
        {
            return IpcError.NullResponse;
        }

        return response;
    }

    public void Dispose()
    {
        _pipe.Dispose();
    }
}
