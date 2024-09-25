using AslHelp.Api.Errors;

namespace AslHelp.Api.Responses;

public enum ResponseCode
{
    [ApiErrorMessage("Unknown error.")]
    Unknown,

    [ApiErrorMessage("Request was successful.")]
    Ok,

    [ApiErrorMessage("Cannot deserialize a null packet.")]
    NullPacket,

    [ApiErrorMessage("Request code was not known by the server.")]
    UnknownRequest,

    [ApiErrorMessage("Request was invalid or malformatted.")]
    InvalidRequest
}
