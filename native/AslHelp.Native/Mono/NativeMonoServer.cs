using AslHelp.Ipc;
using AslHelp.Ipc.Mono;
using AslHelp.Ipc.Mono.Protocol;
using AslHelp.Ipc.Responses;

namespace AslHelp.Native.Mono;

internal sealed class NativeMonoServer : MonoServer
{
    public NativeMonoServer()
        : base(ApiResourceStrings.PipeName) { }

    protected override IpcResult<GetMonoImageExitCode, GetMonoImageResponse> GetMonoImage(GetMonoImageRequest request)
    {
        Output.Log($"[GetMonoImage] Request: {request.Name}");

        nuint image = MonoApi.MonoImage_Loaded(request.Name);
        if (image == 0)
        {
            Output.Log("[GetMonoImage]   => Failure!");
            return GetMonoImageExitCode.NotFound;
        }

        Output.Log($"[GetMonoImage]   => Success: 0x{image:X}.");

        return new GetMonoImageResponse(
            image,
            MonoApi.MonoImage_GetName(image),
            MonoApi.MonoImage_GetName(image) + ".dll",
            MonoApi.MonoImage_GetFileName(image));
    }

    protected override IpcResult<GetMonoClassResponse> GetMonoClass(GetMonoClassRequest request)
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
}
