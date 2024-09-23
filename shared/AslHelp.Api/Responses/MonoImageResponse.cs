namespace AslHelp.Api.Responses;

public sealed record MonoImageResponse(
    ulong Address,
    string Name,
    string ModuleName,
    string FileName);
