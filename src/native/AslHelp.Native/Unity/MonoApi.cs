using AslHelp.Engines.Unity;

namespace AslHelp.Ipc.Native.Unity;

internal sealed class MonoApi(
    nint hMonoModule) : IUnityApi
{
    public MonoGetRootDomain MonoGetRootDomain { get; }
        = ImportResolver.Load<MonoGetRootDomain>(hMonoModule, "mono_get_root_domain");

    public MonoImageLoaded MonoImageLoaded { get; }
        = ImportResolver.Load<MonoImageLoaded>(hMonoModule, "mono_image_loaded");

    public MonoImageGetName MonoImageGetName { get; }
        = ImportResolver.Load<MonoImageGetName>(hMonoModule, "mono_image_get_name");

    public MonoImageGetFilename MonoImageGetFilename { get; }
        = ImportResolver.Load<MonoImageGetFilename>(hMonoModule, "mono_image_get_filename");

    public MonoClassGet MonoClassGet { get; }
        = ImportResolver.Load<MonoClassGet>(hMonoModule, "mono_class_get");

    public MonoClassFromNameCase MonoClassFromNameCase { get; }
        = ImportResolver.Load<MonoClassFromNameCase>(hMonoModule, "mono_class_from_name_case");

    public MonoClassVTable MonoClassVTable { get; }
        = ImportResolver.Load<MonoClassVTable>(hMonoModule, "mono_class_vtable");
}
