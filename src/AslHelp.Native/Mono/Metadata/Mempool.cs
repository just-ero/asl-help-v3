// Generated from Unity 2017.1 Mono headers using ClangSharpPInvokeGenerator.
//
// https://github.com/Unity-Technologies/mono
// https://github.com/dotnet/ClangSharp

namespace AslHelp.Native.Mono.Metadata;

/// <remarks>
///     <see href="https://github.com/Unity-Technologies/mono/blob/unity-2017.1-mbe/mono/metadata/mempool.c#L57-L77"/>
/// </remarks>
internal unsafe struct MonoMemPool
{
    public MonoMemPool* Next;
    public uint Size;
    public byte* Pos;
    public byte* End;
    public DUnion D;

    public struct DUnion
    {
        public double Pad;
        public uint Allocated;
    }
}
