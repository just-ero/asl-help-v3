using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

using AslHelp.Ipc.Native.Mono.Metadata;

namespace AslHelp.Ipc.Native.Mono.Api;

public static unsafe class MonoType
{
    private static readonly delegate* unmanaged<nuint, MonoTypeNameFormat, byte*> _mono_type_get_name_full;

    static MonoType()
    {
        if (NativeLibrary.TryLoad("mono.dll", out var hModule)
            || NativeLibrary.TryLoad("mono-2.0-bdwgc.dll", out hModule))
        {
            _mono_type_get_name_full = (delegate* unmanaged<nuint, MonoTypeNameFormat, byte*>)NativeLibrary.GetExport(hModule, "mono_type_get_name_full");
        }
        else if (NativeLibrary.TryLoad("GameAssembly.dll", out hModule))
        {
            _mono_type_get_name_full = (delegate* unmanaged<nuint, MonoTypeNameFormat, byte*>)NativeLibrary.GetExport(hModule, "il2cpp_type_get_name_full");
        }
    }

    public static string GetNameFull(nuint type, MonoTypeNameFormat format)
    {
        return Utf8StringMarshaller.ConvertToManaged(_mono_type_get_name_full(type, format))!;
    }
}
