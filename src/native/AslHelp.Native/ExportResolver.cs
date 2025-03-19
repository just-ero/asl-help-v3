using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

using AslHelp.Shared;

namespace AslHelp.Ipc.Native;

internal static class ExportResolver
{
    public static TDelegate GetExport<TDelegate>(nint handle, string entryPoint)
        where TDelegate : Delegate
    {
        nint pImport = NativeLibrary.GetExport(handle, entryPoint);
        return Marshal.GetDelegateForFunctionPointer<TDelegate>(pImport);
    }

    public static bool TryGetExport<TDelegate>(nint handle, string entryPoint, [NotNullWhen(true)] out TDelegate? import)
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
