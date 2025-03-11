using System.Text.Json.Serialization;

using AslHelp.Ipc.Mono.Transmission;

namespace AslHelp.Ipc.Mono;

[JsonSerializable(typeof(IpcRequestMessage<IMonoRequest<IMonoResponse>>))]
[JsonSerializable(typeof(IpcResponseMessage<IMonoResponse>))]
public sealed partial class MonoSerializerContext : JsonSerializerContext;
