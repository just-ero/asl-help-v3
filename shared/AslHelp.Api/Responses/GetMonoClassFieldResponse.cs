namespace AslHelp.Api.Responses;

public sealed class GetMonoClassFieldResponse(
    ulong address,
    int offset,
    string name,
    string typeName) : IResponse
{
    public ulong Address { get; } = address;
    public int Offset { get; } = offset;
    public string Name { get; } = name;
    public string TypeName { get; } = typeName;

    public override string ToString()
    {
        return $"{nameof(GetMonoClassFieldResponse)} {{ {nameof(Address)} = 0x{Address:X}, {nameof(Offset)} = 0x{Offset:X}, {nameof(Name)} = {Name}, {nameof(TypeName)} = {TypeName} }}";
    }
}
