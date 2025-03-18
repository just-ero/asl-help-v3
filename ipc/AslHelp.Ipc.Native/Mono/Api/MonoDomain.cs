using System.Runtime.InteropServices;

namespace AslHelp.Ipc.Native.Mono.Api;

public static unsafe class MonoDomain
{
    private static readonly delegate* unmanaged<nuint> _mono_get_root_domain;

    static MonoDomain()
    {
        if (NativeLibrary.TryLoad("mono.dll", out var hModule)
            || NativeLibrary.TryLoad("mono-2.0-bdwgc.dll", out hModule))
        {
            _mono_get_root_domain = (delegate* unmanaged<nuint>)NativeLibrary.GetExport(hModule, "mono_get_root_domain");
        }
        else if (NativeLibrary.TryLoad("GameAssembly.dll", out hModule))
        {
            _mono_get_root_domain = (delegate* unmanaged<nuint>)NativeLibrary.GetExport(hModule, "il2cpp_domain_get");
        }
    }

    public static nuint GetRootDomain()
    {
        return _mono_get_root_domain();
    }
}
