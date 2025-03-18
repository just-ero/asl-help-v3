using AslHelp.Shared.Results;
using AslHelp.Shared.Results.Errors;

namespace AslHelp.Ipc.Mono.Commands;

public sealed record GetMonoFieldRequest(
    ulong Klass,
    string Name) : IMonoRequest<GetMonoFieldResponse>
{
    public IResult<GetMonoFieldResponse> Handle(IMonoVisitor visitor)
    {
        return visitor.GetMonoField(this);
    }
}

public sealed record GetMonoFieldResponse(
    ulong Address,
    uint Offset,
    string Type) : IMonoResponse;

public sealed record GetMonoFieldError : ResultError
{
    private GetMonoFieldError(string message)
        : base(message) { }

    public static GetMonoFieldError NotFound(string name)
    {
        return new($"Field '{name}' not found.");
    }
}
