using System.Text.Json.Serialization;

using AslHelp.Ipc.Mono.Transmission.Commands;

namespace AslHelp.Ipc.Mono.Transmission;

[JsonDerivedType(typeof(GetMonoImageResponse), nameof(GetMonoImageResponse))]
public interface IMonoResponse;
