using System.Text.Json.Serialization;

using AslHelp.Ipc.Responses;

namespace AslHelp.Ipc;

[JsonSerializable(typeof(IpcExitCode))]
internal sealed partial class ApiSerializerContext : JsonSerializerContext;
