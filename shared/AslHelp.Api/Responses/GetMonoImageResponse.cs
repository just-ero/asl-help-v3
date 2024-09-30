namespace AslHelp.Api.Responses;

public sealed class GetMonoImageResponse(
    ulong address,
    string name,
    string moduleName,
    string fileName) : IResponse
{
    public ulong Address { get; } = address;
    public string Name { get; } = name;
    public string ModuleName { get; } = moduleName;
    public string FileName { get; } = fileName;

    public override string ToString()
    {
        return $"{nameof(GetMonoImageResponse)} {{ {nameof(Address)} = 0x{Address:X}, {nameof(Name)} = {Name}, {nameof(ModuleName)} = {ModuleName}, {nameof(FileName)} = {FileName} }}";
    }
}
