// Generated from Unity 2017.1 Mono headers using ClangSharpPInvokeGenerator.
//
// https://github.com/Unity-Technologies/mono
// https://github.com/dotnet/ClangSharp

namespace AslHelp.Native.Mono.Utils;

/// <remarks>
///     <see href="https://github.com/Unity-Technologies/mono/blob/unity-2017.1-mbe/mono/utils/mono-internal-hash.h#L40-L48"/>
/// </remarks>
internal unsafe struct MonoInternalHashTable
{
    public delegate* unmanaged[Cdecl]<void*, uint> HashFunc;
    public delegate* unmanaged[Cdecl]<void*, void*> KeyExtract;
    public delegate* unmanaged[Cdecl]<void*, void**> NextValue;

    public int Size;
    public int NumEntries;
    public void** Table;
}
