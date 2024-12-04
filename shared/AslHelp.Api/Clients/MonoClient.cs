using System.IO.Pipes;

using AslHelp.Api.Requests;
using AslHelp.Api.Responses;
using AslHelp.Shared.Results;

namespace AslHelp.Api.Clients;

public sealed class MonoClient : BaseClient
{
    public MonoClient(string pipeName)
        : base(pipeName) { }

    public MonoClient(string pipeName, PipeOptions options)
        : base(pipeName, options) { }

    public Result<GetMonoImageResponse> GetMonoImage(string fileName)
    {
        GetMonoImageRequest request = new(fileName);
        return Exchange<GetMonoImageRequest, GetMonoImageResponse>(request);
    }

    public Result<GetMonoClassResponse> GetMonoClass(ulong imageAddress, string name)
    {
        return GetMonoClass(imageAddress, "", name);
    }

    public Result<GetMonoClassResponse> GetMonoClass(ulong imageAddress, string nameSpace, string className)
    {
        GetMonoClassRequest request = new(imageAddress, nameSpace, className);
        return Exchange<GetMonoClassRequest, GetMonoClassResponse>(request);
    }
}
