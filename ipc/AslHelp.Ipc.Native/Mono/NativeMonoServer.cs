using System.IO;

using AslHelp.IO.Logging;
using AslHelp.Ipc.Mono;
using AslHelp.Ipc.Mono.Commands;
using AslHelp.Ipc.Native.Mono.Metadata;
using AslHelp.Shared.Results;

namespace AslHelp.Ipc.Native.Mono;

internal sealed class NativeMonoServer(Logger? logger = null) : MonoServer(IpcConnection.PipeName, logger)
{
    protected override Result<GetMonoImageResponse> GetMonoImage(GetMonoImageRequest request)
    {
        Logger?.LogDetail($"Getting image '{request.Name}'...");

        nuint address = Api.MonoImage.Loaded(
            request.Name);

        if (address == 0)
        {
            Logger?.LogDetail("  => Not found.");
            return GetMonoImageError.NotFound(request.Name);
        }

        Logger?.LogDetail($"  => Success: 0x{address:X}.");

        string fileName = Api.MonoImage.GetFileName(address);
        string moduleName = Path.GetFileName(fileName);

        return new GetMonoImageResponse(
            address,
            request.Name,
            moduleName,
            fileName);
    }

    protected override Result<GetMonoClassResponse> GetMonoClass(GetMonoClassRequest request)
    {
        Logger?.LogDetail($"Getting class '{request.Name}'...");

        nuint address = Api.MonoClass.FromName(
            (nuint)request.Image,
            request.Namespace,
            request.Name);

        if (address == 0)
        {
            Logger?.LogDetail("  => Not found.");
            return GetMonoClassError.NotFound(request.Name);
        }

        Logger?.LogDetail($"  => Success: 0x{address:X}.");

        nuint vtableAddress = Api.MonoClass.GetVTable(address);
        if (vtableAddress == 0)
        {
            Logger?.LogDetail("  => VTable null.");
            return GetMonoClassError.NotFound(request.Name);
        }

        Logger?.LogDetail($"  => VTable: 0x{vtableAddress:X}.");

        nuint staticFieldData = Api.MonoVTable.GetStaticFieldData(vtableAddress);
        return new GetMonoClassResponse(
            address,
            staticFieldData);
    }

    protected override Result<GetMonoFieldResponse> GetMonoField(GetMonoFieldRequest request)
    {
        Logger?.LogDetail($"Getting field '{request.Name}'...");

        nuint address = Api.MonoField.FromName(
            (nuint)request.Klass,
            request.Name);

        if (address == 0)
        {
            Logger?.LogDetail("  => Not found.");
            return GetMonoFieldError.NotFound(request.Name);
        }

        Logger?.LogDetail($"  => Success: 0x{address:X}.");

        nuint typeAddress = Api.MonoField.GetType(address);
        if (typeAddress == 0)
        {
            Logger?.LogDetail("  => Type null.");
            return GetMonoFieldError.NotFound(request.Name);
        }

        return new GetMonoFieldResponse(
            address,
            Api.MonoField.GetOffset(address),
            Api.MonoType.GetNameFull(typeAddress, MonoTypeNameFormat.Il));
    }
}
