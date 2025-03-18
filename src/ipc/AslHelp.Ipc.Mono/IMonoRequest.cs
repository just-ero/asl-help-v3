using System.Text.Json.Serialization;

using AslHelp.Ipc.Mono.Commands;

namespace AslHelp.Ipc.Mono;

[JsonDerivedType(typeof(GetMonoImageRequest), nameof(GetMonoImageRequest))]
[JsonDerivedType(typeof(GetMonoClassRequest), nameof(GetMonoClassRequest))]
[JsonDerivedType(typeof(GetMonoFieldRequest), nameof(GetMonoFieldRequest))]
public interface IMonoRequest<out TResponse> : IRequest<TResponse, IMonoVisitor>;
