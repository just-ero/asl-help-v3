namespace AslHelp.Api.Requests;

public sealed class GetMonoClassRequest(
    ulong image,
    string @namespace,
    string name) : IApiPacket
{
    public ulong Image { get; } = image;
    public string Namespace { get; } = @namespace;
    public string Name { get; } = name;
}
