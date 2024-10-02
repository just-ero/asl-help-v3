using System;
using System.Collections.Generic;
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

    public Result<IEnumerable<Result<TResponse>>> ExchangeMany<TRequest, TResponse>(TRequest request)
        where TRequest : IRequest
        where TResponse : IResponse
    {
        SendRequest(request.Code);

        ResponseCode responseCode = ApiSerializer.SendPacket(_pipe, request);
        if (responseCode != ResponseCode.Ok)
        {
            return ApiError.FromResponseCode(responseCode);
        }

        return Result<IEnumerable<Result<TResponse>>>.Ok(ReceiveMany<TResponse>());
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

    private IEnumerable<Result<TResponse>> ReceiveMany<TResponse>()
        where TResponse : IResponse
    {
        try
        {
            while (ReceiveResponse() == ResponseCode.EnumerableMore)
            {
                TResponse? response = ApiSerializer.ReceivePacket<TResponse>(_pipe);
                if (response is null)
                {
                    yield return ApiError.FromResponseCode(ResponseCode.InvalidPacket);
                }
                else
                {
                    yield return response;
                }

                SendRequest(RequestCode.EnumerableContinue);
            }
        }
        finally
        {
            ApiSerializer.Serialize(_pipe, RequestCode.EnumerableBreak);
        }
    }
}
