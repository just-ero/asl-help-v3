using System.IO;

using AslHelp.Interop.Mono;
using AslHelp.IO.Logging;
using AslHelp.Ipc.Mono;
using AslHelp.Ipc.Mono.Commands;
using AslHelp.Shared.Results;

namespace AslHelp.Interop.Native.Mono;

internal sealed unsafe class NativeMonoServer(
    IMonoApiSet api,
    Logger? logger = null) : MonoServer("asl-help-ipc", logger)
{
    protected override Result<GetMonoImageResponse> GetMonoImage(GetMonoImageRequest request)
    {
        Logger?.LogDetail($"Getting image '{request.Name}'...");

        nint pImage = api.MonoImageLoaded(
            request.Name);

        if (pImage == 0)
        {
            Logger?.LogDetail("  => Not found.");
            return GetMonoImageError.NotFound(request.Name);
        }

        Logger?.LogDetail($"  => Success: 0x{pImage:X}.");

        string fileName = api.MonoImageGetFilename(pImage);
        string moduleName = Path.GetFileName(fileName);

        return new GetMonoImageResponse(
            pImage,
            request.Name,
            moduleName,
            fileName);
    }

    protected override Result<GetMonoClassResponse> GetMonoClass(GetMonoClassRequest request)
    {
        Logger?.LogDetail($"Getting class '{request.Name}'...");

        nint pClass = api.MonoClassFromNameCase(
            (nint)request.Image,
            request.Namespace,
            request.Name);

        if (pClass == 0)
        {
            Logger?.LogDetail("  => Not found.");
            return GetMonoClassError.NotFound(request.Name);
        }

        Logger?.LogDetail($"  => Success: 0x{pClass:X}.");

        nint pVTable = api.MonoClassVTable(api.MonoGetRootDomain(), pClass);
        if (pVTable == 0)
        {
            Logger?.LogDetail("  => VTable null.");
            return GetMonoClassError.NotFound(request.Name);
        }

        Logger?.LogDetail($"  => VTable: 0x{pVTable:X}.");

        nint staticFieldData = api.MonoVTableGetStaticFieldData(pVTable);
        return new GetMonoClassResponse(
            pClass,
            staticFieldData);
    }

    protected override Result<GetMonoFieldResponse> GetMonoField(GetMonoFieldRequest request)
    {
        Logger?.LogDetail($"Getting field '{request.Name}'...");

        nint pField = api.MonoClassGetFieldFromName(
            (nint)request.Klass,
            request.Name);

        if (pField == 0)
        {
            Logger?.LogDetail("  => Not found.");
            return GetMonoFieldError.NotFound(request.Name);
        }

        Logger?.LogDetail($"  => Success: 0x{pField:X}.");

        nint pType = api.MonoFieldGetType(pField);
        if (pType == 0)
        {
            Logger?.LogDetail("  => Type null.");
            return GetMonoFieldError.NotFound(request.Name);
        }

        return new GetMonoFieldResponse(
            pField,
            api.MonoFieldGetOffset(pField),
            api.MonoTypeGetName(pType));
    }
}
