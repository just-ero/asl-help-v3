// Generated from Unity 2017.1 Mono headers using ClangSharpPInvokeGenerator.
//
// https://github.com/Unity-Technologies/mono
// https://github.com/dotnet/ClangSharp

using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

using unsafe MonoClassVTable = delegate* unmanaged<nint, nint, nint>;
using unsafe MonoClassGet = delegate* unmanaged<nint, uint, nint>;
using unsafe MonoClassFromNameCase = delegate* unmanaged<nint, byte*, byte*, nint>;

namespace AslHelp.Ipc.Native.Mono.Api;

public static unsafe class MonoClass
{
    private static readonly MonoClassVTable _class_vtable;
    private static readonly MonoClassGet _class_get;
    private static readonly MonoClassFromNameCase _mono_class_from_name_case;

    static MonoClass()
    {
        if (NativeLibrary.TryLoad("mono.dll", out var hModule)
            || NativeLibrary.TryLoad("mono-2.0-bdwgc.dll", out hModule))
        {
            _class_vtable = (MonoClassVTable)NativeLibrary.GetExport(hModule, "mono" + nameof(_class_vtable));
            _class_get = (MonoClassGet)NativeLibrary.GetExport(hModule, "mono_class_get");
            _mono_class_from_name_case = (MonoClassFromNameCase)NativeLibrary.GetExport(hModule, "mono_class_from_name_case");
        }
        else if (NativeLibrary.TryLoad("GameAssembly.dll", out hModule))
        {
            _class_vtable = (MonoClassVTable)NativeLibrary.GetExport(hModule, "il2cpp" + nameof(_class_vtable));
            _class_get = (MonoClassGet)NativeLibrary.GetExport(hModule, "il2cpp" + nameof(_class_get));
            _mono_class_from_name_case = (MonoClassFromNameCase)NativeLibrary.GetExport(hModule, "il2cpp_class_from_name_case");
        }
    }

    public static nint FromToken(nint image, uint type_token)
    {
        return _class_get(image, type_token);
    }

    public static nint FromName(nint image, string @namespace, string name)
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

    public static nint GetVTable(nint klass)
    {
        return _class_vtable(MonoDomain.GetRootDomain(), klass);
    }
}
