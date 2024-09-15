// Generated from Unity 2017.1 Mono headers using ClangSharpPInvokeGenerator.
//
// https://github.com/Unity-Technologies/mono
// https://github.com/dotnet/ClangSharp

namespace AslHelp.Native.Mono.Utils;

/// <remarks>
///     <see href="https://github.com/Unity-Technologies/mono/blob/unity-2017.1-mbe/mono/utils/mono-codeman.c#L72-L80">utils/mono-codeman.c</see>
/// </remarks>
internal unsafe struct CodeChunk
{
    public sbyte* Data;
    public int Pos;
    public int Size;
    public CodeChunk* Next;
    private uint _bitfield0;

    public uint Flags
    {
        readonly get => _bitfield0 & 0xFFu;
        set => _bitfield0 = (_bitfield0 & ~0xFFu) | (value & 0xFFu);
    }

    public uint BSize
    {
        readonly get => (_bitfield0 >> 8) & 0xFFFFFFu;
        set => _bitfield0 = (_bitfield0 & ~(0xFFFFFFu << 8)) | ((value & 0xFFFFFFu) << 8);
    }
}

/// <remarks>
///     <see href="https://github.com/Unity-Technologies/mono/blob/unity-2017.1-mbe/mono/utils/mono-codeman.c#L82-L88">utils/mono-codeman.c</see>
/// </remarks>
internal unsafe struct MonoCodeManager
{
    public int Dynamic;
    public int ReadOnly;
    public CodeChunk* Current;
    public CodeChunk* Full;
    public CodeChunk* Last;
}
