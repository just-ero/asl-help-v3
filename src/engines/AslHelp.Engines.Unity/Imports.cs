using System.Runtime.InteropServices;

namespace AslHelp.Engines.Unity;

// MonoImage
[UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
public unsafe delegate nint MonoImageLoaded(string name);

[UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
public unsafe delegate string MonoImageGetName(nint pImage);

[UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
public unsafe delegate string MonoImageGetFilename(nint pImage);

// MonoClass
[UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
public unsafe delegate nint MonoClassFromName(nint image, string name_space, string name);

// [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
// public unsafe delegate nint MonoClassFromMonoType(nint image, nint type);
