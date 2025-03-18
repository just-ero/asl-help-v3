using System.Runtime.InteropServices;

namespace AslHelp.Engines.Unity;

// MonoDomain
[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
public delegate nint MonoGetRootDomain();

// MonoImage
[UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
public delegate nint MonoImageLoaded(string name);

[UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
public delegate string MonoImageGetName(nint pImage);

[UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
public delegate string MonoImageGetFilename(nint pImage);

// MonoClass
[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
public delegate nint MonoClassGet(nint image, uint typeToken);

[UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
public delegate nint MonoClassFromNameCase(nint image, string name_space, string name);

[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
public delegate nint MonoClassVTable(nint domain, nint klass);
