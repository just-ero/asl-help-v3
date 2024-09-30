namespace AslHelp.Api.Responses;

public sealed class GetMonoClassFieldResponse(
    ulong address,
    int offset,
    string name) : IResponse
{
    public ulong Address { get; } = address;
    public int Offset { get; } = offset;
    public string Name { get; } = name;

    public override string ToString()
    {
        return $"{nameof(GetMonoClassFieldResponse)} {{ {nameof(Address)} = 0x{Address:X}, {nameof(Offset)} = 0x{Offset:X}, {nameof(Name)} = {Name} }}";
    }
}
