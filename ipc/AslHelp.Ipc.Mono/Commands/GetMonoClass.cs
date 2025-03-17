using AslHelp.Shared.Results;
using AslHelp.Shared.Results.Errors;

namespace AslHelp.Ipc.Mono.Commands;

public sealed record GetMonoClassRequest(
    ulong Image,
    string Namespace,
    string Name) : IMonoRequest<GetMonoClassResponse>
{
    public IResult<GetMonoClassResponse> Handle(IMonoVisitor visitor)
    {
        return visitor.GetMonoClass(this);
    }
}

public sealed record GetMonoClassResponse(
    ulong Address) : IMonoResponse;

public sealed record GetMonoClassError : ResultError
{
    private GetMonoClassError(string message)
        : base(message) { }

    public static GetMonoClassError NotFound(string name)
    {
        return new($"Class '{name}' not found.");
    }
}
