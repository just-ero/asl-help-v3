namespace AslHelp.Api.Responses;

public sealed class GetMonoClassResponse(
    ulong address) : IResponse
{
    public ulong Address { get; } = address;

    public override string ToString()
    {
        return $"{nameof(GetMonoClassResponse)} {{ {nameof(Address)} = 0x{Address:X} }}";
    }
}
