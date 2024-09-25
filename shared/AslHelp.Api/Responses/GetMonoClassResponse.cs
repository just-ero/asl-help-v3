namespace AslHelp.Api.Responses;

public sealed record GetMonoClassResponse(
    ulong Address) : IPacket;
