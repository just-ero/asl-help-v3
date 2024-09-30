namespace AslHelp.Api.Responses;

public enum ResponseCode : byte
{
    Unknown,

    Ok,

    EnumerableMore,
    EnumerableEnd,

    UnknownRequest,
    InvalidPacket
}
