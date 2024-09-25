using System.IO.Pipes;

using AslHelp.Api.Errors;
using AslHelp.Api.Requests;
using AslHelp.Api.Responses;
using AslHelp.Shared.Results;

namespace AslHelp.Api.Clients;

public sealed class MonoClient : BaseClient
{
    public MonoClient()
        : base(ApiResourceStrings.PipeName) { }

    public MonoClient(string pipeName)
        : base(pipeName, PipeOptions.None) { }

    public MonoClient(PipeOptions options)
        : base(ApiResourceStrings.PipeName, options) { }

    public MonoClient(string pipeName, PipeOptions options)
        : base(pipeName, options) { }

    public Result<GetMonoImageResponse> GetMonoImage(GetMonoImageRequest request)
    {
        ApiSerializer.Serialize(_pipe, RequestCode.GetMonoImage);

        var responseCode = ApiSerializer.SendPacket(_pipe, request);
        if (responseCode == ResponseCode.Ok)
        {
            var response = ApiSerializer.ReceivePacket<GetMonoImageResponse>(_pipe);
            return response is not null
                ? response
                : ApiError.FromResponseCode(ResponseCode.NullPacket);
        }

        return ApiError.FromResponseCode(responseCode);
    }

    public Result<GetMonoClassResponse> GetMonoClass(GetMonoClassRequest request)
    {
        ApiSerializer.Serialize(_pipe, RequestCode.GetMonoClass);

        var responseCode = ApiSerializer.SendPacket(_pipe, request);
        if (responseCode == ResponseCode.Ok)
        {
            var response = ApiSerializer.ReceivePacket<GetMonoClassResponse>(_pipe);
            return response is not null
                ? response
                : ApiError.FromResponseCode(ResponseCode.NullPacket);
        }

        return ApiError.FromResponseCode(responseCode);
    }
}
