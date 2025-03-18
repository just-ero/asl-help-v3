namespace AslHelp.Engines.Unity;

public interface IUnityApi
{
    MonoImageLoaded MonoImageLoaded { get; }
    MonoImageGetName MonoImageGetName { get; }
    MonoImageGetFilename MonoImageGetFilename { get; }
}
