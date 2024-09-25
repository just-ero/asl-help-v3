namespace AslHelp.Api.Requests;

public sealed record GetMonoClassRequest(
    ulong Image,
    string Namespace,
    string Name) : IPacket;
