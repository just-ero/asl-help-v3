namespace AslHelp.Interop.Mono;

public unsafe interface IMonoApiSet
{
    nint MonoGetRootDomain();

    nint MonoImageLoaded(string name);
    string MonoImageGetFilename(nint pImage);

    nint MonoClassGet(nint pImage, uint typeToken);
    nint MonoClassFromNameCase(nint pKlass, string @namespace, string name);
    nint MonoClassGetFieldFromName(nint pKlass, string name);
    nint MonoClassVTable(nint pDomain, nint pKlass);

    nint MonoFieldGetType(nint pField);
    uint MonoFieldGetOffset(nint pField);

    string MonoTypeGetName(nint pType);

    nint MonoVTableGetStaticFieldData(nint pVTable);
}
