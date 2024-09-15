using System.Runtime.InteropServices;

namespace AslHelp.Native.Mono;

internal static unsafe partial class MonoApi
{
    private const string Mono = "mono.dll";

    [LibraryImport(Mono, EntryPoint = nameof(mono_image_loaded))]
    public static partial void* mono_image_loaded(sbyte* name);

    [LibraryImport(Mono, EntryPoint = nameof(mono_class_from_name_case))]
    public static partial void* mono_class_from_name_case(void* image, sbyte* name_space, sbyte* name);
}
