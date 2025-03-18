// Generated from Unity 2017.1 Mono headers using ClangSharpPInvokeGenerator.
//
// https://github.com/Unity-Technologies/mono
// https://github.com/dotnet/ClangSharp

using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

namespace AslHelp.Ipc.Native.Mono.Api;

public static unsafe class MonoImage
{
    private static readonly delegate* unmanaged<byte*, nuint> _mono_image_loaded;
    private static readonly delegate* unmanaged<nuint, byte*> _mono_image_get_name;
    private static readonly delegate* unmanaged<nuint, byte*> _mono_image_get_filename;

    static MonoImage()
    {
        if (NativeLibrary.TryLoad("mono.dll", out var hModule)
            || NativeLibrary.TryLoad("mono-2.0-bdwgc.dll", out hModule))
        {
            _mono_image_loaded = (delegate* unmanaged<byte*, nuint>)NativeLibrary.GetExport(hModule, "mono_image_loaded");
            _mono_image_get_name = (delegate* unmanaged<nuint, byte*>)NativeLibrary.GetExport(hModule, "mono_image_get_name");
            _mono_image_get_filename = (delegate* unmanaged<nuint, byte*>)NativeLibrary.GetExport(hModule, "mono_image_get_filename");
        }
        else if (NativeLibrary.TryLoad("GameAssembly.dll", out hModule))
        {
            _mono_image_loaded = (delegate* unmanaged<byte*, nuint>)NativeLibrary.GetExport(hModule, "il2cpp_image_loaded");
            _mono_image_get_name = (delegate* unmanaged<nuint, byte*>)NativeLibrary.GetExport(hModule, "il2cpp_image_get_name");
            _mono_image_get_filename = (delegate* unmanaged<nuint, byte*>)NativeLibrary.GetExport(hModule, "il2cpp_image_get_filename");
        }
    }

    public static nuint Loaded(string name)
    {
        scoped Utf8StringMarshaller.ManagedToUnmanagedIn marshaller = default;
        try
        {
            marshaller.FromManaged(name, stackalloc byte[Utf8StringMarshaller.ManagedToUnmanagedIn.BufferSize]);
            return _mono_image_loaded(marshaller.ToUnmanaged());
        }
        finally
        {
            marshaller.Free();
        }
    }

    public static string GetName(nuint address)
    {
        return Utf8StringMarshaller.ConvertToManaged(_mono_image_get_name(address))!;
    }

    public static string GetFileName(nuint address)
    {
        return Utf8StringMarshaller.ConvertToManaged(_mono_image_get_filename(address))!;
    }
}

