using System.Text.Json.Serialization;

using AslHelp.Ipc.Mono.Transmission.Commands;

namespace AslHelp.Ipc.Mono.Transmission;

[JsonDerivedType(typeof(GetMonoImageRequest), nameof(GetMonoImageRequest))]
public interface IMonoRequest<out TResponse> : IRequest<TResponse, IMonoVisitor>;
