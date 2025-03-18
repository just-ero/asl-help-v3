using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

namespace AslHelp.Ipc.Native.Mono.Api;

public static unsafe class MonoField
{
    private static readonly delegate* unmanaged<nuint, byte*, nuint> _mono_class_get_field_from_name;
    private static readonly delegate* unmanaged<nuint, byte*> _mono_field_get_name;
    private static readonly delegate* unmanaged<nuint, nuint> _mono_field_get_type;
    private static readonly delegate* unmanaged<nuint, uint> _mono_field_get_offset;

    static MonoField()
    {
        if (NativeLibrary.TryLoad("mono.dll", out var hModule)
            || NativeLibrary.TryLoad("mono-2.0-bdwgc.dll", out hModule))
        {
            _mono_class_get_field_from_name = (delegate* unmanaged<nuint, byte*, nuint>)NativeLibrary.GetExport(hModule, "mono_class_get_field_from_name");
            _mono_field_get_name = (delegate* unmanaged<nuint, byte*>)NativeLibrary.GetExport(hModule, "mono_field_get_name");
            _mono_field_get_type = (delegate* unmanaged<nuint, nuint>)NativeLibrary.GetExport(hModule, "mono_field_get_type");
            _mono_field_get_offset = (delegate* unmanaged<nuint, uint>)NativeLibrary.GetExport(hModule, "mono_field_get_offset");
        }
        else if (NativeLibrary.TryLoad("GameAssembly.dll", out hModule))
        {
            _mono_class_get_field_from_name = (delegate* unmanaged<nuint, byte*, nuint>)NativeLibrary.GetExport(hModule, "il2cpp_class_get_field_from_name");
            _mono_field_get_name = (delegate* unmanaged<nuint, byte*>)NativeLibrary.GetExport(hModule, "il2cpp_field_get_name");
            _mono_field_get_type = (delegate* unmanaged<nuint, nuint>)NativeLibrary.GetExport(hModule, "il2cpp_field_get_type");
            _mono_field_get_offset = (delegate* unmanaged<nuint, uint>)NativeLibrary.GetExport(hModule, "il2cpp_field_get_offset");
        }
    }

    public static nuint FromName(nuint klass, string name)
    {
        scoped Utf8StringMarshaller.ManagedToUnmanagedIn marshaller = default;
        try
        {
            marshaller.FromManaged(name, stackalloc byte[Utf8StringMarshaller.ManagedToUnmanagedIn.BufferSize]);
            return _mono_class_get_field_from_name(klass, marshaller.ToUnmanaged());
        }
        finally
        {
            marshaller.Free();
        }
    }

    public static string GetName(nuint field)
    {
        return Utf8StringMarshaller.ConvertToManaged(_mono_field_get_name(field))!;
    }

    public static nuint GetType(nuint field)
    {
        return _mono_field_get_type(field);
    }

    public static uint GetOffset(nuint field)
    {
        return _mono_field_get_offset(field);
    }
}
