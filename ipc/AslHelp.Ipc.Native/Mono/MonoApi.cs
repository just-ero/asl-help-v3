using System.Runtime.InteropServices;

namespace AslHelp.Ipc.Native.Mono;

/// <summary>
///     Provides marshalled access to the Mono API.
/// </summary>
internal static unsafe partial class MonoApi
{
    private const string Mono = "mono.dll";

    // MonoImage
    [LibraryImport(Mono, EntryPoint = "mono_image_loaded", StringMarshalling = StringMarshalling.Utf8)]
    public static partial nuint MonoImage_Loaded(
        string name);

    [LibraryImport(Mono, EntryPoint = "mono_image_get_name", StringMarshalling = StringMarshalling.Utf8)]
    [return: MarshalAs(UnmanagedType.LPUTF8Str)]
    public static partial string MonoImage_GetName(
        nuint image);

    [LibraryImport(Mono, EntryPoint = "mono_image_get_filename", StringMarshalling = StringMarshalling.Utf8)]
    [return: MarshalAs(UnmanagedType.LPUTF8Str)]
    public static partial string MonoImage_GetFileName(
        nuint image);

    // MonoClass
    [LibraryImport(Mono, EntryPoint = "mono_class_get")]
    public static partial nuint MonoClass_FromToken(
        nuint image,
        uint type_token);

    [LibraryImport(Mono, EntryPoint = "mono_class_from_name_case", StringMarshalling = StringMarshalling.Utf8)]
    public static partial nuint MonoClass_FromName(
        nuint image,
        string name_space,
        string name);

    // MonoType
    [LibraryImport(Mono, EntryPoint = "mono_type_get_name_full", StringMarshalling = StringMarshalling.Utf8)]
    [return: MarshalAs(UnmanagedType.LPUTF8Str)]
    public static partial string MonoType_GetName(
        nuint type,
        int format);
}
