using AslHelp.Shared.Results;

namespace AslHelp.Ipc.Mono.Commands;

public sealed record GetMonoImageRequest(
    string Name) : IMonoRequest<GetMonoImageResponse>
{
    public IResult<GetMonoImageResponse> Visit(IMonoVisitor visitor)
    {
        return visitor.GetMonoImage(this);
    }
}

public sealed record GetMonoImageResponse(
    long Address,
    string Name,
    string ModuleName,
    string FileName) : IMonoResponse;
