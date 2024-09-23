// Generated from glibc headers using ClangSharpPInvokeGenerator.
//
// https://sourceware.org/git/glibc.git
// https://github.com/dotnet/ClangSharp

namespace AslHelp.Native.Glibc.Sysdeps.Nptl.Bits;

/// <remarks>
///     <see href="https://sourceware.org/git/glibc.git/?a=blob;f=sysdeps/nptl/bits/struct_mutex.h#l27"/>
/// </remarks>
internal struct PThreadMutexS
{
    public int Lock;
    public uint Count;
    public int Owner;
    public uint Nusers;
    public int Kind;
    public short Spins;
    public short Elision;
    public PThreadInternalList List;
}
