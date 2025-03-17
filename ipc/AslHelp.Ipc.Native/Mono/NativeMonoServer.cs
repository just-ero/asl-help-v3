using AslHelp.IO.Logging;
using AslHelp.Ipc;
using AslHelp.Ipc.Mono;
using AslHelp.Ipc.Mono.Commands;
using AslHelp.Shared.Results;

namespace AslHelp.Native.Mono;

internal sealed class NativeMonoServer(Logger? logger = null) : MonoServer(IpcConnection.PipeName, logger)
{
    protected override Result<GetMonoImageResponse> GetMonoImage(GetMonoImageRequest request)
    {
        Logger?.LogDetail($"Getting image '{request.Name}'...");

        nuint image = MonoApi.MonoImage_Loaded(request.Name);
        if (image == 0)
        {
            Logger?.LogDetail("   => Failure!");
            return GetMonoImageError.NotFound(request.Name);
        }

        Logger?.LogDetail($"   => Success: 0x{image:X}.");

        return new GetMonoImageResponse(
            image,
            request.Name,
            request.Name + ".dll",
            MonoApi.MonoImage_GetFileName(image));
    }
}
