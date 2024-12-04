using System;
using System.IO.Pipes;

using AslHelp.Api.Requests;
using AslHelp.Api.Responses;

namespace AslHelp.Api.Servers;

public class BaseServer : IDisposable
{
    private readonly NamedPipeServerStream _pipe;

    public BaseServer(string pipeName)
        : this(pipeName, PipeOptions.None) { }

    public BaseServer(string pipeName, PipeOptions options)
    {
        _pipe = new(pipeName, PipeDirection.InOut, 1, PipeTransmissionMode.Byte, options);
    }

    public bool IsConnected { get; private set; }

    protected virtual void ProcessRequest(RequestCode code)
    {
        switch (code)
        {
            case RequestCode.Close:
                IsConnected = false;
                break;
        }
    }

    public RequestCode ProcessNextRequest()
    {
        RequestCode code = ReceiveRequest();
        ProcessRequest(code);

        return code;
    }

    public void SendResponse(ResponseCode responseCode)
    {
        ApiSerializer.Serialize(_pipe, responseCode);
    }

    public RequestCode ReceiveRequest()
    {
        return ApiSerializer.Deserialize<RequestCode>(_pipe);
    }

    public void Exchange<TRequest, TResponse>(Func<TRequest, TResponse> transform)
        where TRequest : IRequest
        where TResponse : IResponse
    {
        if (ApiSerializer.ReceivePacket<TRequest>(_pipe) is { } request)
        {
            ApiSerializer.SendPacket(_pipe, transform(request));
        }
    }

    public void WaitForConnection()
    {
        _pipe.WaitForConnection();
        IsConnected = true;
    }

    public void Dispose()
    {
        if (_pipe.IsConnected)
        {
            _pipe.Dispose();
        }

        IsConnected = false;
    }
}
