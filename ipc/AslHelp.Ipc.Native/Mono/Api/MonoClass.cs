// Generated from Unity 2017.1 Mono headers using ClangSharpPInvokeGenerator.
//
// https://github.com/Unity-Technologies/mono
// https://github.com/dotnet/ClangSharp

using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

namespace AslHelp.Ipc.Native.Mono.Api;

public static unsafe class MonoClass
{
    private static readonly delegate* unmanaged<nuint, nuint, nuint> _mono_class_vtable;
    private static readonly delegate* unmanaged<nuint, uint, nuint> _mono_class_get;
    private static readonly delegate* unmanaged<nuint, byte*, byte*, nuint> _mono_class_from_name_case;

    static MonoClass()
    {
        if (NativeLibrary.TryLoad("mono.dll", out var hModule)
            || NativeLibrary.TryLoad("mono-2.0-bdwgc.dll", out hModule))
        {
            _mono_class_vtable = (delegate* unmanaged<nuint, nuint, nuint>)NativeLibrary.GetExport(hModule, "mono_class_vtable");
            _mono_class_get = (delegate* unmanaged<nuint, uint, nuint>)NativeLibrary.GetExport(hModule, "mono_class_get");
            _mono_class_from_name_case = (delegate* unmanaged<nuint, byte*, byte*, nuint>)NativeLibrary.GetExport(hModule, "mono_class_from_name_case");
        }
        else if (NativeLibrary.TryLoad("GameAssembly.dll", out hModule))
        {
            _mono_class_vtable = (delegate* unmanaged<nuint, nuint, nuint>)NativeLibrary.GetExport(hModule, "il2cpp_class_vtable");
            _mono_class_get = (delegate* unmanaged<nuint, uint, nuint>)NativeLibrary.GetExport(hModule, "il2cpp_class_from_system");
            _mono_class_from_name_case = (delegate* unmanaged<nuint, byte*, byte*, nuint>)NativeLibrary.GetExport(hModule, "il2cpp_class_from_name_case");
        }
    }

    public static nuint FromToken(nuint image, uint type_token)
    {
        return _mono_class_get(image, type_token);
    }

    public static nuint FromName(nuint image, string @namespace, string name)
    {
        scoped Utf8StringMarshaller.ManagedToUnmanagedIn namespaceMarshaller = default;
        scoped Utf8StringMarshaller.ManagedToUnmanagedIn nameMarshaller = default;
        try
        {
            namespaceMarshaller.FromManaged(@namespace, stackalloc byte[Utf8StringMarshaller.ManagedToUnmanagedIn.BufferSize]);
            nameMarshaller.FromManaged(name, stackalloc byte[Utf8StringMarshaller.ManagedToUnmanagedIn.BufferSize]);
            return _mono_class_from_name_case(image, namespaceMarshaller.ToUnmanaged(), nameMarshaller.ToUnmanaged());
        }
        finally
        {
            namespaceMarshaller.Free();
            nameMarshaller.Free();
        }
    }

    public static nuint GetVTable(nuint klass)
    {
        return _mono_class_vtable(MonoDomain.GetRootDomain(), klass);
    }
}
