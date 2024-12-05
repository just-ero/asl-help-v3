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
        while (_pipe.IsConnected)
        {
            RequestCode request = ApiSerializer.Deserialize<RequestCode>(_pipe);

            if (request == RequestCode.Close)
            {
                // ?
            }
        }
    }

    protected virtual void ProcessRequest(RequestCode code) { }

    public void Exchange<TRequest, TResponse>(Func<TRequest, TResponse> transform)
        where TRequest : IRequest
        where TResponse : IResponse
    {
        TRequest? request = ApiSerializer.Deserialize<TRequest>(_pipe);

        if (request is not null)
        {
            ApiSerializer.SendPacket(_pipe, transform(request));
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
