using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

using AslHelp.Shared;

namespace AslHelp.Ipc.Native;

internal static class ImportResolver
{
    public static TDelegate Load<TDelegate>(nint handle, string entryPoint)
        where TDelegate : Delegate
    {
        if (!NativeLibrary.TryGetExport(handle, entryPoint, out nint pImport))
        {
            ThrowHelper.ThrowEntryPointNotFoundException(entryPoint);
        }

        return Marshal.GetDelegateForFunctionPointer<TDelegate>(pImport);
    }

    public static bool TryLoad<TDelegate>(nint handle, string entryPoint, [NotNullWhen(true)] out TDelegate? import)
        where TDelegate : Delegate
    {
        if (!NativeLibrary.TryGetExport(handle, entryPoint, out nint pImport))
        {
            import = null;
            return false;
        }

        import = Marshal.GetDelegateForFunctionPointer<TDelegate>(pImport);
        return true;
    }
}
