using System.Runtime.InteropServices;

using AslHelp.Native.Mono.Metadata;

namespace AslHelp.Native.Mono;

internal static unsafe partial class MonoApi
{
    private const string Mono = "mono.dll";

    [LibraryImport(Mono, EntryPoint = nameof(mono_image_loaded), StringMarshalling = StringMarshalling.Utf8)]
    public static partial MonoImage* mono_image_loaded(string name);

    [LibraryImport(Mono, EntryPoint = nameof(mono_class_from_name_case), StringMarshalling = StringMarshalling.Utf8)]
    public static partial MonoClass* mono_class_from_name_case(MonoImage* image, string name_space, string name);

    [LibraryImport(Mono, EntryPoint = nameof(mono_class_get_fields))]
    public static partial MonoClassField* mono_class_get_fields(MonoClass* klass, ref nuint iter);
}
