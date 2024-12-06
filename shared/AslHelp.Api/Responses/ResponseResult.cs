namespace AslHelp.Api.Responses;

public sealed class ResponseResult
{
    public ResponseResult(ResponseCode code)
    {
        Code = code;
    }

    public ResponseResult(IResponse response)
    {
        Code = ResponseCode.Ok;
        Response = response;
    }

    public ResponseCode Code { get; }
    public IResponse? Response { get; }

    public void Deconstruct(out ResponseCode code, out IResponse? response)
    {
        code = Code;
        response = Response;
    }
}
