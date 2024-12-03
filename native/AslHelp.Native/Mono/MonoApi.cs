using System.Runtime.InteropServices;

namespace AslHelp.Native.Mono;

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

    [LibraryImport(Mono, EntryPoint = "mono_image_get_name")]
    [return: MarshalAs(UnmanagedType.LPStr)]
    public static partial string MonoImage_GetName(
        nuint image);

    [LibraryImport(Mono, EntryPoint = "mono_image_get_filename")]
    [return: MarshalAs(UnmanagedType.LPStr)]
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

    [LibraryImport(Mono, EntryPoint = "mono_type_get_name_full")]
    [return: MarshalAs(UnmanagedType.LPStr)]
    public static partial string MonoType_GetName(
        nuint type,
        int format);
}
