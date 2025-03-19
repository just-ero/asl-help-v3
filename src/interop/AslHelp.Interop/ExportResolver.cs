using System.Runtime.InteropServices;
using System.Security;

using AslHelp.Shared;

namespace AslHelp.Ipc.Native;

public static unsafe class NativeLibrary
{
    public static nint Load(string libraryPath)
    {
        ThrowHelper.ThrowIfNull(libraryPath);

        nint handle;
        fixed (char* pLibraryPath = libraryPath)
        {
            handle = LoadLibraryW((ushort*)pLibraryPath);
        }

        if (handle == 0)
        {
            ThrowHelper.ThrowWin32Exception();
        }

        return handle;
    }

    public static bool TryLoad(string libraryPath, out nint handle)
    {
        ThrowHelper.ThrowIfNull(libraryPath);

        fixed (char* pLibraryPath = libraryPath)
        {
            handle = LoadLibraryW((ushort*)pLibraryPath);
        }

        return handle != 0;
    }

    public static nint GetExport(nint handle, string name)
    {
        ThrowHelper.ThrowIfNull(handle);
        ThrowHelper.ThrowIfNull(name);

        nint address = GetProcAddress(handle, name);
        if (address == 0)
        {
            ThrowHelper.ThrowWin32Exception();
        }

        return address;
    }

    public static bool TryGetExport(nint handle, string name, out nint address)
    {
        ThrowHelper.ThrowIfNull(handle);
        ThrowHelper.ThrowIfNull(name);

        address = GetProcAddress(handle, name);
        return address != 0;
    }

    [DllImport("kernel32", EntryPoint = nameof(LoadLibraryW), ExactSpelling = true, SetLastError = true)]
    [SuppressUnmanagedCodeSecurity]
    private static extern nint LoadLibraryW(ushort* lpLibFileName);

    [DllImport("kernel32", EntryPoint = nameof(GetProcAddress), ExactSpelling = true, SetLastError = true)]
    [SuppressUnmanagedCodeSecurity]
    private static extern nint GetProcAddress(nint hModule, string procName);
}
