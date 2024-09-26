using AslHelp.Api.Responses;
using AslHelp.Shared.Results.Errors;

namespace AslHelp.Api.Errors;

internal sealed record ApiError : ResultError
{
    private ApiError(string message)
        : base(message) { }

    public static ApiError Other(string message)
    {
        return new(message);
    }

    public static ApiError FromResponseCode(ResponseCode responseCode)
    {
        return responseCode switch
        {
            ResponseCode.Unknown => Unknown,
            ResponseCode.Ok => Ok,
            ResponseCode.UnknownRequest => UnknownRequest,
            ResponseCode.InvalidRequest => InvalidRequest,
            _ => new($"Server responded '{responseCode}'.")
        };
    }

    public static ApiError Unknown
        => new("Unknown error.");

    public static ApiError Ok
        => new("Request was successful.");

    public static ApiError UnknownRequest
        => new("Request code was not known by the server.");

    public static ApiError InvalidRequest
        => new("Request was null or otherwise invalid.");
}
