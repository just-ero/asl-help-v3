using System.Text.Json.Serialization;

namespace AslHelp.Ipc.Mono;

[JsonSerializable(typeof(IpcRequestMessage<IMonoRequest<IMonoResponse>>))]
[JsonSerializable(typeof(IpcResponseMessage<IMonoResponse>))]
public sealed partial class MonoSerializerContext : JsonSerializerContext;
