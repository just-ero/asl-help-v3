using AslHelp.Engines.Unity;

namespace AslHelp.Ipc.Native.Unity;

internal sealed class MonoApi(
    nint hMonoModule) : IUnityApi
{
    public MonoImageLoaded MonoImageLoaded { get; }
        = ImportResolver.Load<MonoImageLoaded>(hMonoModule, "mono_image_loaded");

    public MonoImageGetName MonoImageGetName { get; }
        = ImportResolver.Load<MonoImageGetName>(hMonoModule, "mono_image_get_name");

    public MonoImageGetFilename MonoImageGetFilename { get; }
        = ImportResolver.Load<MonoImageGetFilename>(hMonoModule, "mono_image_get_filename");
}
