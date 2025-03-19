using AslHelp.Ipc.Native;

namespace AslHelp.Interop.Mono;

public sealed unsafe class MonoApiSet(
    nint monoModuleHandle)
    : IMonoApiSet
{
    private readonly mono_class_from_name_case _pMonoClassFromNameCase
        = (mono_class_from_name_case)NativeLibrary.GetExport(monoModuleHandle, "mono_class_from_name_case");

    public nint MonoClassFromNameCase(nint pKlass, string @namespace, string name)
    {
        ansistring
        return _pMonoClassFromNameCase(pKlass, @namespace, name);
    }

    public nint MonoClassGet(nint pImage, uint typeToken)
    {
        throw new System.NotImplementedException();
    }

    public nint MonoClassGetFieldFromName(nint pKlass, string name)
    {
        throw new System.NotImplementedException();
    }

    public nint MonoClassVTable(nint pDomain, nint pKlass)
    {
        throw new System.NotImplementedException();
    }

    public uint MonoFieldGetOffset(nint pField)
    {
        throw new System.NotImplementedException();
    }

    public nint MonoFieldGetType(nint pField)
    {
        throw new System.NotImplementedException();
    }

    public nint MonoGetRootDomain()
    {
        throw new System.NotImplementedException();
    }

    public string MonoImageGetFilename(nint pImage)
    {
        throw new System.NotImplementedException();
    }

    public nint MonoImageLoaded(string name)
    {
        throw new System.NotImplementedException();
    }

    public string MonoTypeGetName(nint pType)
    {
        throw new System.NotImplementedException();
    }

    public nint MonoVTableGetStaticFieldData(nint pVTable)
    {
        throw new System.NotImplementedException();
    }
}
