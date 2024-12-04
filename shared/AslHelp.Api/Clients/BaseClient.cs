using System;
using System.IO.Pipes;

using AslHelp.Api.Errors;
using AslHelp.Api.Requests;
using AslHelp.Api.Responses;
using AslHelp.Shared.Results;

namespace AslHelp.Api.Clients;

public class BaseClient : IDisposable
{
    private readonly NamedPipeClientStream _pipe;

    public BaseClient(string pipeName)
        : this(pipeName, PipeOptions.None) { }

    public BaseClient(string pipeName, PipeOptions options)
    {
        _pipe = new(".", pipeName, PipeDirection.InOut, options);
    }

    public bool IsConnected { get; private set; }

    public void SendRequest(RequestCode code)
    {
        ApiSerializer.Serialize(_pipe, code);
    }

    public ResponseCode ReceiveResponse()
    {
        return ApiSerializer.Deserialize<ResponseCode>(_pipe);
    }

    public Result<TResponse> Exchange<TRequest, TResponse>(TRequest request)
        where TRequest : IRequest
        where TResponse : IResponse
    {
        SendRequest(request.Code);

        ResponseCode responseCode = ApiSerializer.SendPacket(_pipe, request);
        if (responseCode != ResponseCode.Ok)
        {
            return ApiError.FromResponseCode(responseCode);
        }

        TResponse? response = ApiSerializer.ReceivePacket<TResponse>(_pipe);
        return response is not null
            ? response
            : ApiError.FromResponseCode(ResponseCode.InvalidPacket);
    }

    public void Connect(int timeout = -1)
    {
        _pipe.Connect(timeout);
        IsConnected = true;
    }

    public void Dispose()
    {
        if (_pipe.IsConnected)
        {
            SendRequest(RequestCode.Close);
            _pipe.Dispose();
        }

        IsConnected = false;
    }
}
