using System.Collections.Generic;
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

    public sealed override void ProcessRequest(RequestCode code)
    {
        switch (code)
        {
            case RequestCode.GetMonoImage:
                Exchange<GetMonoImageRequest, GetMonoImageResponse>(GetMonoImage);
                break;
            case RequestCode.GetMonoClass:
                Exchange<GetMonoClassRequest, GetMonoClassResponse>(GetMonoClass);
                break;
            case RequestCode.GetMonoClassFields:
                ExchangeMany<GetMonoClassFieldsRequest, GetMonoClassFieldResponse>(GetMonoClassFields);
                break;
        }

        base.ProcessRequest(code);
    }

    protected abstract GetMonoImageResponse GetMonoImage(GetMonoImageRequest request);
    protected abstract GetMonoClassResponse GetMonoClass(GetMonoClassRequest request);
    protected abstract IEnumerable<GetMonoClassFieldResponse> GetMonoClassFields(GetMonoClassFieldsRequest request);
}
