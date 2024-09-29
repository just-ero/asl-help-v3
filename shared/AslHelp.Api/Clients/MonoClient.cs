using System.IO.Pipes;

using AslHelp.Api.Errors;
using AslHelp.Api.Requests;
using AslHelp.Api.Responses;
using AslHelp.Shared.Results;

namespace AslHelp.Api.Clients;

public sealed class MonoClient : BaseClient
{
    public MonoClient(string pipeName)
        : base(pipeName, PipeOptions.None) { }

    public MonoClient(string pipeName, PipeOptions options)
        : base(pipeName, options) { }

    public Result<GetMonoImageResponse> GetMonoImage(GetMonoImageRequest request)
    {
        ApiSerializer.Serialize(_pipe, RequestCode.GetMonoImage);

        ResponseCode responseCode = ApiSerializer.SendPacket(_pipe, request);
        if (responseCode == ResponseCode.Ok)
        {
            GetMonoImageResponse? response = ApiSerializer.ReceivePacket<GetMonoImageResponse>(_pipe);
            return response is not null
                ? response
                : ApiError.FromResponseCode(ResponseCode.InvalidRequest);
        }

        return ApiError.FromResponseCode(responseCode);
    }

    public Result<GetMonoClassResponse> GetMonoClass(GetMonoClassRequest request)
    {
        ApiSerializer.Serialize(_pipe, RequestCode.GetMonoClass);

        ResponseCode responseCode = ApiSerializer.SendPacket(_pipe, request);
        if (responseCode == ResponseCode.Ok)
        {
            GetMonoClassResponse? response = ApiSerializer.ReceivePacket<GetMonoClassResponse>(_pipe);
            return response is not null
                ? response
                : ApiError.FromResponseCode(ResponseCode.InvalidRequest);
        }

        return ApiError.FromResponseCode(responseCode);
    }
}
