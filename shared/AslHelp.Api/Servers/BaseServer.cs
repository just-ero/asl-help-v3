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

    public void Start()
    {
        while (true)
        {
            _pipe.WaitForConnection();

            while (_pipe.IsConnected)
            {
                RequestCode request = ApiSerializer.Deserialize<RequestCode>(_pipe);

                if (request == RequestCode.Close)
                {
                    _pipe.Disconnect();
                    break;
                }

                (ResponseCode, IRequest?) d = ProcessRequest(request);
                ApiSerializer.Serialize(_pipe, response.Code);

                if (response is { Code: ResponseCode.Ok, Response: { } payload })
                {
                    ApiSerializer.Serialize(_pipe, payload);
                }
            }
        }
    }

    protected virtual ResponseResult ProcessRequest(RequestCode request)
    {
        return new(ResponseCode.Ok);
    }

    public void Exchange<TRequest, TResponse>(Func<TRequest, TResponse> transform)
        where TRequest : IRequest
        where TResponse : IResponse
    {
        TRequest? request = ApiSerializer.Deserialize<TRequest>(_pipe);

        if (request is not null)
        {
            ApiSerializer.Serialize(_pipe, transform(request));
        }
    }

    public void WaitForConnection()
    {
        if (!IsConnected)
        {
            _pipe.WaitForConnection();
            IsConnected = true;
        }
    }

    public void Dispose()
    {
        if (IsConnected)
        {
            _pipe.Dispose();
            IsConnected = false;
        }
    }
}
