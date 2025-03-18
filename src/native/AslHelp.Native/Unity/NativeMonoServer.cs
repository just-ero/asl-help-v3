using System.IO;

using AslHelp.Engines.Unity;
using AslHelp.IO.Logging;
using AslHelp.Ipc.Mono;
using AslHelp.Ipc.Mono.Commands;
using AslHelp.Ipc.Native.Mono.Metadata;
using AslHelp.Shared.Results;

namespace AslHelp.Ipc.Native.Mono;

internal sealed class NativeMonoServer(
    IUnityApi api,
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

        nint pClass = Api.MonoClass.FromName(
            (nint)request.Image,
            request.Namespace,
            request.Name);

        if (pClass == 0)
        {
            Logger?.LogDetail("  => Not found.");
            return GetMonoClassError.NotFound(request.Name);
        }

        Logger?.LogDetail($"  => Success: 0x{pClass:X}.");

        nint pVTable = Api.MonoClass.GetVTable(pClass);
        if (pVTable == 0)
        {
            Logger?.LogDetail("  => VTable null.");
            return GetMonoClassError.NotFound(request.Name);
        }

        Logger?.LogDetail($"  => VTable: 0x{pVTable:X}.");

        nint staticFieldData = Api.MonoVTable.GetStaticFieldData(pVTable);
        return new GetMonoClassResponse(
            pClass,
            staticFieldData);
    }

    protected override Result<GetMonoFieldResponse> GetMonoField(GetMonoFieldRequest request)
    {
        Logger?.LogDetail($"Getting field '{request.Name}'...");

        nint pField = Api.MonoField.FromName(
            (nint)request.Klass,
            request.Name);

        if (pField == 0)
        {
            Logger?.LogDetail("  => Not found.");
            return GetMonoFieldError.NotFound(request.Name);
        }

        Logger?.LogDetail($"  => Success: 0x{pField:X}.");

        nint pType = Api.MonoField.GetType(pField);
        if (pType == 0)
        {
            Logger?.LogDetail("  => Type null.");
            return GetMonoFieldError.NotFound(request.Name);
        }

        return new GetMonoFieldResponse(
            pField,
            Api.MonoField.GetOffset(pField),
            Api.MonoType.GetNameFull(pType, MonoTypeNameFormat.Il));
    }
}
