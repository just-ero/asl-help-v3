using AslHelp.Ipc;
using AslHelp.Ipc.Requests;
using AslHelp.Ipc.Responses;
using AslHelp.Ipc.Servers;

namespace AslHelp.Native.Mono;

internal sealed class NativeMonoServer : MonoServerBase
{
    public NativeMonoServer()
        : base(ApiResourceStrings.PipeName) { }

    protected override ActionResult<GetMonoClassResponse> GetMonoClass(GetMonoClassRequest request)
    {
        Output.Log($"[GetMonoClass] Request: {string.Join('.', request.Namespace, request.Name)}");

        nuint klass = MonoApi.MonoClass_FromName((nuint)request.Image, request.Namespace, request.Name);
        if (klass == 0)
        {
            Output.Log("[GetMonoClass]   => Failure!");
            return IpcExitCode.MonoClass_NotFound;
        }

        Output.Log($"[GetMonoClass]   => Success: 0x{klass:X}.");

        return new GetMonoClassResponse(
            klass);
    }

    protected override ActionResult<GetMonoImageResponse> GetMonoImage(GetMonoImageRequest request)
    {
        Output.Log($"[GetMonoImage] Request: {request.Name}");

        nuint image = MonoApi.MonoImage_Loaded(request.Name);
        if (image == 0)
        {
            Output.Log("[GetMonoImage]   => Failure!");
            return IpcExitCode.MonoImage_NotFound;
        }

        Output.Log($"[GetMonoImage]   => Success: 0x{image:X}.");

        return new GetMonoImageResponse(
            image,
            MonoApi.MonoImage_GetName(image),
            MonoApi.MonoImage_GetName(image) + ".dll",
            MonoApi.MonoImage_GetFileName(image));
    }
}
