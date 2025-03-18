using System.Runtime.InteropServices;

namespace AslHelp.Ipc.Native.Mono.Api;

public static unsafe class MonoVTable
{
    private static readonly delegate* unmanaged<nint, nint> _mono_vtable_get_static_field_data;

    static MonoVTable()
    {
        if (NativeLibrary.TryLoad("mono.dll", out var hModule)
            || NativeLibrary.TryLoad("mono-2.0-bdwgc.dll", out hModule))
        {
            _mono_vtable_get_static_field_data = (delegate* unmanaged<nint, nint>)NativeLibrary.GetExport(hModule, "mono_vtable_get_static_field_data");
        }
        else if (NativeLibrary.TryLoad("GameAssembly.dll", out hModule))
        {
            _mono_vtable_get_static_field_data = (delegate* unmanaged<nint, nint>)NativeLibrary.GetExport(hModule, "il2cpp_class_get_static_field_data");
        }
    }

    public static nint GetStaticFieldData(nint vtable)
    {
        return _mono_vtable_get_static_field_data(vtable);
    }
}
