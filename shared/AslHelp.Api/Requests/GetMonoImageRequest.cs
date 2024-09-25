namespace AslHelp.Api.Requests;

public sealed record GetMonoImageRequest(
    string NameOrPath) : IPacket;
