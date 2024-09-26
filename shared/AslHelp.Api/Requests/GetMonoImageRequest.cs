namespace AslHelp.Api.Requests;

public sealed class GetMonoImageRequest(
    string nameOrPath) : IApiPacket
{
    public string NameOrPath { get; } = nameOrPath;
}
