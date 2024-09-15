// Generated from glibc headers using ClangSharpPInvokeGenerator.
//
// https://sourceware.org/git/glibc.git
// https://github.com/dotnet/ClangSharp

using System.Runtime.InteropServices;

namespace AslHelp.Native.Glibc.Sysdeps.Nptl.Bits;

/// <remarks>
///     <see href="https://sourceware.org/git/glibc.git/?a=blob;f=sysdeps/nptl/bits/pthreadtypes.h#l67"/>
/// </remarks>
[StructLayout(LayoutKind.Explicit)]
internal unsafe struct PThreadMutexT
{
    public const int SizeofPThreadMutexT = 40;

    [FieldOffset(0)]
    public PThreadMutexS Data;

    [FieldOffset(0)]
    public fixed sbyte Size[SizeofPThreadMutexT];

    [FieldOffset(0)]
    public int Align;
}
