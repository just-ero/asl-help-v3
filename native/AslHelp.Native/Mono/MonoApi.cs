using System.Runtime.InteropServices;

namespace AslHelp.Native.Mono;

internal static unsafe partial class MonoApi
{
    private const string Mono = "mono.dll";

    [LibraryImport(Mono, EntryPoint = nameof(mono_image_loaded), StringMarshalling = StringMarshalling.Utf8)]
    public static partial nuint mono_image_loaded(string name);

    [LibraryImport(Mono, EntryPoint = nameof(mono_class_from_name_case), StringMarshalling = StringMarshalling.Utf8)]
    public static partial void* mono_class_from_name_case(nuint image, string name_space, string name);
}
