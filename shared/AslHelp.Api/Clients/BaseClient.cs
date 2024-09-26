using System;
using System.IO.Pipes;

using AslHelp.Api.Requests;
using AslHelp.Api.Responses;

namespace AslHelp.Api.Clients;

public abstract class BaseClient : IDisposable
{
    protected readonly NamedPipeClientStream _pipe;

    protected BaseClient(string pipeName)
        : this(pipeName, PipeOptions.None) { }

    protected BaseClient(string pipeName, PipeOptions options)
    {
        _pipe = new(".", pipeName, PipeDirection.InOut, options);
    }

    public void SendRequest(RequestCode code)
    {
        ApiSerializer.Serialize(_pipe, code);
    }

    public ResponseCode ReceiveResponse()
    {
        return ApiSerializer.Deserialize<ResponseCode>(_pipe);
    }

    public void Connect(int timeout = -1)
    {
        _pipe.Connect(timeout);
    }

    public void Dispose()
    {
        if (_pipe.IsConnected)
        {
            _pipe.Dispose();
        }
    }
}
