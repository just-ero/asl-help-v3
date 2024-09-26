namespace AslHelp.Api.Responses;

public sealed class GetMonoClassResponse(
    ulong address) : IApiPacket
{
    public ulong Address { get; } = address;
}
