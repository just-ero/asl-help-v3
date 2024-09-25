namespace AslHelp.Api.Responses;

public record GetMonoImageResponse(
    ulong Address,
    string Name,
    string ModuleName,
    string FileName) : IPacket;
