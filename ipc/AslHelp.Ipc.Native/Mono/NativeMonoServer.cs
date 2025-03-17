using System.IO;

using AslHelp.IO.Logging;
using AslHelp.Ipc.Mono;
using AslHelp.Ipc.Mono.Commands;
using AslHelp.Shared.Results;

namespace AslHelp.Ipc.Native.Mono;

internal sealed class NativeMonoServer(Logger? logger = null) : MonoServer(IpcConnection.PipeName, logger)
{
    protected override Result<GetMonoImageResponse> GetMonoImage(GetMonoImageRequest request)
    {
        Logger?.LogDetail($"Getting image '{request.Name}'...");

        nuint image = MonoApi.MonoImage_Loaded(
            request.Name);

        if (image == 0)
        {
            Logger?.LogDetail("  => Not found.");
            return GetMonoImageError.NotFound(request.Name);
        }

        Logger?.LogDetail($"  => Success: 0x{image:X}.");

        string fileName = MonoApi.MonoImage_GetFileName(image);
        string moduleName = Path.GetFileName(fileName);

        return new GetMonoImageResponse(
            image,
            request.Name,
            moduleName,
            fileName);
    }

    protected override Result<GetMonoClassResponse> GetMonoClass(GetMonoClassRequest request)
    {
        Logger?.LogDetail($"Getting class '{request.Name}'...");

        nuint klass = MonoApi.MonoClass_FromName(
            (nuint)request.Image,
            request.Namespace,
            request.Name);

        if (klass == 0)
        {
            Logger?.LogDetail("  => Failure!");
            return GetMonoClassError.NotFound(request.Name);
        }

        Logger?.LogDetail($"  => Success: 0x{klass:X}.");

        return new GetMonoClassResponse(
            klass);
    }
}
