using System.Text.Json.Serialization;

using AslHelp.Ipc.Mono.Requests;
using AslHelp.Ipc.Mono.Responses;

namespace AslHelp.Ipc.Mono;

[JsonSerializable(typeof(IpcRequestMessage<MonoRequestBase>))]
[JsonSerializable(typeof(IpcRequestMessage<GetMonoImageRequest>))]

[JsonSerializable(typeof(IpcRequestMessage<MonoResponseBase>))]
[JsonSerializable(typeof(IpcRequestMessage<GetMonoImageResponse>))]
public sealed partial class MonoSerializerContext : JsonSerializerContext;
