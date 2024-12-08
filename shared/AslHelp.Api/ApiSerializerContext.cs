using System.Text.Json.Serialization;

using AslHelp.Api.Requests;
using AslHelp.Api.Responses;

namespace AslHelp.Api;

[JsonSerializable(typeof(RequestCode))]
[JsonSerializable(typeof(GetMonoImageRequest))]
[JsonSerializable(typeof(GetMonoClassRequest))]

[JsonSerializable(typeof(ResponseCode))]
[JsonSerializable(typeof(GetMonoImageResponse))]
[JsonSerializable(typeof(GetMonoClassResponse))]
internal sealed partial class ApiSerializerContext : JsonSerializerContext;
