using System.Text.Json.Serialization;

using AslHelp.Ipc.Requests;
using AslHelp.Ipc.Responses;

namespace AslHelp.Ipc;

[JsonSerializable(typeof(RequestCode))]
[JsonSerializable(typeof(GetMonoImageRequest))]
[JsonSerializable(typeof(GetMonoClassRequest))]

[JsonSerializable(typeof(IpcExitCode))]
[JsonSerializable(typeof(GetMonoImageResponse))]
[JsonSerializable(typeof(GetMonoClassResponse))]
internal sealed partial class ApiSerializerContext : JsonSerializerContext;
