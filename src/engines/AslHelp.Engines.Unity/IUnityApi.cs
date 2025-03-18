namespace AslHelp.Engines.Unity;

public interface IUnityApi
{
    MonoGetRootDomain MonoGetRootDomain { get; }

    MonoImageLoaded MonoImageLoaded { get; }
    MonoImageGetName MonoImageGetName { get; }
    MonoImageGetFilename MonoImageGetFilename { get; }

    MonoClassGet MonoClassGet { get; }
    MonoClassFromNameCase MonoClassFromNameCase { get; }
    MonoClassVTable MonoClassVTable { get; }
}
