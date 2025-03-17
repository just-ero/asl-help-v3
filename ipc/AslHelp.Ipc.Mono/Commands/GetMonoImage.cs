using AslHelp.Shared.Results;

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
    ulong Address,
    string Name,
    string ModuleName,
    string FileName) : IMonoResponse;
