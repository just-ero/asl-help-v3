using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Reflection;

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

        return Result<IEnumerable<Result<TResponse>>>.Ok(receivePackets());

        IEnumerable<Result<TResponse>> receivePackets()
        {
            try
            {
                while (true)
                {
                    if (ReceiveResponse() is ResponseCode.EnumerableEnd or not ResponseCode.EnumerableMore)
                    {
                        yield break;
                    }

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
