using System.IO.Pipes;

using AslHelp.Api.Requests;
using AslHelp.Api.Responses;

namespace AslHelp.Api.Servers;

public abstract class MonoServerBase : BaseServer
{
    protected MonoServerBase(string pipeName)
        : base(pipeName) { }

    protected MonoServerBase(string pipeName, PipeOptions options)
        : base(pipeName, options) { }

    protected sealed override IResponseResult ProcessRequest(RequestCode code)
    {
        return code switch
        {
            RequestCode.GetMonoImage => Exchange<GetMonoImageRequest, GetMonoImageResponse>(GetMonoImage),
            RequestCode.GetMonoClass => Exchange<GetMonoClassRequest, GetMonoClassResponse>(GetMonoClass),
            _ => Unknown()
        };
    }

    protected abstract ResponseResult<GetMonoImageResponse> GetMonoImage(GetMonoImageRequest request);
    protected abstract ResponseResult<GetMonoClassResponse> GetMonoClass(GetMonoClassRequest request);
}
