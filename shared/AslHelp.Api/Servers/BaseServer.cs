using System;
using System.IO.Pipes;

using AslHelp.Api.Requests;
using AslHelp.Api.Responses;

namespace AslHelp.Api.Servers;

public abstract class BaseServer : IDisposable
{
    private readonly NamedPipeServerStream _pipe;

    protected BaseServer(string pipeName)
        : this(pipeName, PipeOptions.None) { }

    protected BaseServer(string pipeName, PipeOptions options)
    {
        _pipe = new(pipeName, PipeDirection.InOut, 1, PipeTransmissionMode.Byte, options);
    }

    public void SendResponse(ResponseCode responseCode)
    {
        ApiSerializer.Serialize(_pipe, responseCode);
    }

    public RequestCode ReceiveRequest()
    {
        return ApiSerializer.Deserialize<RequestCode>(_pipe);
    }

    public void ProcessRequest(RequestCode requestCode)
    {
        if (requestCode != RequestCode.Close)
        {
            if (!ProcessRequestInternal(requestCode))
            {
                SendResponse(ResponseCode.UnknownRequest);
            }
        }
    }

    protected abstract bool ProcessRequestInternal(RequestCode requestCode);

    protected void Exchange<TRequest, TResponse>(Func<TRequest, TResponse> transform)
        where TRequest : IApiPacket
        where TResponse : IApiPacket
    {
        if (ApiSerializer.ReceivePacket<TRequest>(_pipe) is { } request)
        {
            ApiSerializer.SendPacket(_pipe, transform(request));
        }
    }

    public void WaitForConnection()
    {
        _pipe.WaitForConnection();
    }

    public void Dispose()
    {
        if (_pipe.IsConnected)
        {
            _pipe.Dispose();
        }
    }
}
