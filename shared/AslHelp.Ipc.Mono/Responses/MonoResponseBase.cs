using System.Text.Json.Serialization;

using AslHelp.Ipc.Responses;

namespace AslHelp.Ipc.Mono.Responses;

[JsonDerivedType(typeof(GetMonoImageResponse), nameof(GetMonoImageResponse))]
public abstract record MonoResponseBase : IResponse<IMonoResponseVisitor>
{
    public abstract void Visit(IMonoResponseVisitor visitor);
}
