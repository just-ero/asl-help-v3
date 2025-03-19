using AslHelp.Interop.Mono;

namespace AslHelp.Ipc.Native.Unity;

internal sealed class MonoApi(
    nint hMonoModule) : IMonoApiSet
{
    public MonoGetRootDomain MonoGetRootDomain { get; }
        = ExportResolver.GetExport<MonoGetRootDomain>(hMonoModule, "mono_get_root_domain");

    public MonoImageLoaded MonoImageLoaded { get; }
        = ExportResolver.GetExport<MonoImageLoaded>(hMonoModule, "mono_image_loaded");

    public MonoImageGetName MonoImageGetName { get; }
        = ExportResolver.GetExport<MonoImageGetName>(hMonoModule, "mono_image_get_name");

    public MonoImageGetFilename MonoImageGetFilename { get; }
        = ExportResolver.GetExport<MonoImageGetFilename>(hMonoModule, "mono_image_get_filename");

    public MonoClassGet MonoClassGet { get; }
        = ExportResolver.GetExport<MonoClassGet>(hMonoModule, "mono_class_get");

    public MonoClassFromNameCase MonoClassFromNameCase { get; }
        = ExportResolver.GetExport<MonoClassFromNameCase>(hMonoModule, "mono_class_from_name_case");

    public MonoClassVTable MonoClassVTable { get; }
        = ExportResolver.GetExport<MonoClassVTable>(hMonoModule, "mono_class_vtable");
}
