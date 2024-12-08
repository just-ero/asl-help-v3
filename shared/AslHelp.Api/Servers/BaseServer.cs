using System;
using System.Diagnostics;
using System.IO.Pipes;
using System.Threading.Tasks;

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

    public async Task StartAsync()
    {
        await _pipe.BeginWaitForConnection(
            state =>
            {
                Debug.WriteLine($"{state.AsyncState}");
            },
            null
        );
    }

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

                IResponseResult result = ProcessRequest(request);
                if (result is { ResponseCode: ResponseCode.Ok, Response: { } payload })
                {
                    ApiSerializer.Serialize(_pipe, result.ResponseCode);
                    ApiSerializer.Serialize(_pipe, payload);
                }
                else
                {
                    ApiSerializer.Serialize(_pipe, result.ResponseCode);
                }
            }
        }
    }

    protected virtual IResponseResult ProcessRequest(RequestCode request)
    {
        return Ok();
    }

    public IResponseResult Exchange<TRequest, TResponse>(Func<TRequest, ResponseResult<TResponse>> transform)
        where TRequest : IRequest
        where TResponse : IResponse
    {
        TRequest? request = ApiSerializer.Deserialize<TRequest>(_pipe);

        if (request is null)
        {
            return Invalid();
        }

        return transform(request);
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

    protected ResponseResult Ok()
    {
        return ResponseCode.Ok;
    }

    protected ResponseResult<TResponse> Ok<TResponse>(TResponse response)
        where TResponse : IResponse
    {
        return response;
    }

    protected ResponseResult Error(ResponseCode responseCode)
    {
        return responseCode;
    }

    protected ResponseResult<TResponse> Error<TResponse>(ResponseCode responseCode)
        where TResponse : IResponse
    {
        return responseCode;
    }

    protected ResponseResult Invalid()
    {
        return ResponseCode.InvalidPacket;
    }

    protected ResponseResult Unknown()
    {
        return ResponseCode.Unknown;
    }
}
