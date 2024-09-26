namespace AslHelp.Api.Responses;

public sealed class GetMonoImageResponse(
    ulong address,
    string name,
    string moduleName,
    string fileName) : IApiPacket
{
    public ulong Address { get; } = address;
    public string Name { get; } = name;
    public string ModuleName { get; } = moduleName;
    public string FileName { get; } = fileName;
}
