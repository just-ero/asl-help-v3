using System.Text.Json.Serialization;

using AslHelp.Ipc.Mono.Commands;

namespace AslHelp.Ipc.Mono;

[JsonDerivedType(typeof(GetMonoImageResponse), nameof(GetMonoImageResponse))]
[JsonDerivedType(typeof(GetMonoClassResponse), nameof(GetMonoClassResponse))]
public interface IMonoResponse;
