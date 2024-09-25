using AslHelp.Api.Responses;
using AslHelp.Shared.Extensions;
using AslHelp.Shared.Results.Errors;

namespace AslHelp.Api.Errors;

internal sealed record ApiError : ResultError
{
    public ApiError(string message)
        : base(message) { }

    public static ApiError Other(string message)
    {
        return new(message);
    }

    public static ApiError FromResponseCode(ResponseCode responseCode)
    {
        var attr = responseCode.GetAttribute<ResponseCode, ApiErrorMessageAttribute>();
        return attr is not null
            ? new(attr.Message)
            : new($"Unknown response code '{responseCode}'.");
    }
}
