using AslHelp.Ipc.Mono;
using AslHelp.Ipc.Mono.Commands;
using AslHelp.Shared.Results;

namespace AslHelp.Native.Mono;

internal sealed class NativeMonoServer() : MonoServer("asl-help")
{
    protected override Result<GetMonoImageResponse> GetMonoImage(GetMonoImageRequest request)
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
}
