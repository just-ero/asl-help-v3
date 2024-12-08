namespace AslHelp.Api.Responses;

public interface IResponseResult
{
    ResponseCode ResponseCode { get; }
    IResponse? Response { get; }
}

public readonly struct ResponseResult : IResponseResult
{
    public ResponseResult(ResponseCode responseCode)
    {
        ResponseCode = responseCode;
    }

    public ResponseCode ResponseCode { get; }
    public IResponse? Response => default;

    public static implicit operator ResponseResult(ResponseCode responseCode)
    {
        return new(responseCode);
    }
}

public readonly struct ResponseResult<TResponse> : IResponseResult
    where TResponse : IResponse
{
    public ResponseResult(TResponse response)
    {
        ResponseCode = ResponseCode.Ok;
        Response = response;
    }

    public ResponseResult(ResponseCode responseCode)
    {
        ResponseCode = responseCode;
    }

    public ResponseCode ResponseCode { get; }
    public TResponse? Response { get; }

    IResponse? IResponseResult.Response => Response;

    public static implicit operator ResponseResult<TResponse>(TResponse response)
    {
        return new(response);
    }

    public static implicit operator ResponseResult<TResponse>(ResponseCode responseCode)
    {
        return new(responseCode);
    }
}
