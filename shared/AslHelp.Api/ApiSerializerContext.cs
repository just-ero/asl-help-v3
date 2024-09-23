using System.Text.Json.Serialization;

using AslHelp.Api.Requests;
using AslHelp.Api.Responses;

namespace AslHelp.Api;

[JsonSerializable(typeof(RequestCode))]
[JsonSerializable(typeof(MonoImageRequest))]

[JsonSerializable(typeof(ResponseCode))]
[JsonSerializable(typeof(MonoImageResponse))]
public sealed partial class ApiSerializerContext : JsonSerializerContext;
