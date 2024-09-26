using System;
using System.IO.Pipes;

using AslHelp.Api.Requests;
using AslHelp.Api.Responses;

namespace AslHelp.Api.Servers;

public sealed class MonoServer : BaseServer
{
    public MonoServer(string pipeName)
        : base(pipeName, PipeOptions.None) { }

    public MonoServer(string pipeName, PipeOptions options)
        : base(pipeName, options) { }

    public required Func<GetMonoImageRequest, GetMonoImageResponse> GetMonoImageHandler { get; init; }
    public required Func<GetMonoClassRequest, GetMonoClassResponse> GetMonoClassHandler { get; init; }

    protected override bool ProcessRequestInternal(RequestCode requestCode)
    {
        switch (requestCode)
        {
            case RequestCode.GetMonoImage:
                Exchange(GetMonoImageHandler);
                return true;
            case RequestCode.GetMonoClass:
                Exchange(GetMonoClassHandler);
                return true;
        }

        return false;
    }
}
