using System.Text.Json.Serialization;

using AslHelp.Ipc.Requests;

namespace AslHelp.Ipc.Mono.Requests;

[JsonDerivedType(typeof(GetMonoImageRequest), nameof(GetMonoImageRequest))]
public abstract record MonoRequestBase : IRequest<IMonoRequestVisitor>
{
    public abstract void Visit(IMonoRequestVisitor visitor);
}
