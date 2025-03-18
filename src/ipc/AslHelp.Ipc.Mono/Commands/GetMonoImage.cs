using AslHelp.Shared.Results;
using AslHelp.Shared.Results.Errors;

namespace AslHelp.Ipc.Mono.Commands;

public sealed record GetMonoImageRequest(
    string Name) : IMonoRequest<GetMonoImageResponse>
{
    public IResult<GetMonoImageResponse> Handle(IMonoVisitor visitor)
    {
        return visitor.GetMonoImage(this);
    }
}

public sealed record GetMonoImageResponse(
    long Address,
    string Name,
    string ModuleName,
    string FileName) : IMonoResponse;

public sealed record GetMonoImageError : ResultError
{
    private GetMonoImageError(string message)
        : base(message) { }

    public static GetMonoImageError NotFound(string name)
    {
        return new($"Image '{name}' not found.");
    }
}
