using AslHelp.Api;
using AslHelp.Api.Requests;
using AslHelp.Api.Responses;
using AslHelp.Api.Servers;

namespace AslHelp.Native.Mono;

internal sealed class NativeMonoServer : MonoServerBase
{
    public NativeMonoServer()
        : base(ApiResourceStrings.PipeName) { }

    protected override unsafe GetMonoImageResponse GetMonoImage(GetMonoImageRequest request)
    {
        Output.Log($"[GetMonoImage] Request: {request.Name}");

        nuint image = MonoApi.MonoImage_Loaded(request.Name);
        if (image == 0)
        {
            Output.Log("[GetMonoImage]   => Failure!");
            return new(0, "", "", "");
        }

        Output.Log($"[GetMonoImage]   => Success: 0x{image:X}.");

        return new(
            image,
            MonoApi.MonoImage_GetName(image),
            MonoApi.MonoImage_GetName(image) + ".dll",
            MonoApi.MonoImage_GetFileName(image));
    }

    protected override unsafe GetMonoClassResponse GetMonoClass(GetMonoClassRequest request)
    {
        Output.Log($"[GetMonoClass] Request: {string.Join('.', request.Namespace, request.Name)}");

        nuint klass = MonoApi.MonoClass_FromName((nuint)request.Image, request.Namespace, request.Name);
        if (klass == 0)
        {
            Output.Log("[GetMonoClass]   => Failure!");
            return new(0);
        }

        Output.Log($"[GetMonoClass]   => Success: 0x{(ulong)klass:X}.");

        return new(
            klass);
    }
}
