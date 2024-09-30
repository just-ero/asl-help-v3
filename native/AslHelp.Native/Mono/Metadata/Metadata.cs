// Generated from Unity 2017.1 Mono headers using ClangSharpPInvokeGenerator.
//
// https://github.com/Unity-Technologies/mono
// https://github.com/dotnet/ClangSharp

namespace AslHelp.Native.Mono.Metadata;

/// <remarks>
///     <see href="https://github.com/Unity-Technologies/mono/blob/unity-2017.1-mbe/mono/metadata/metadata.h#L296-L299"/>
/// </remarks>
internal struct MonoCustomMod
{
    private uint _bitfield;

    public uint Required
    {
        readonly get => _bitfield & 0x1u;
        set => _bitfield = (_bitfield & ~0x1u) | (value & 0x1u);
    }

    public uint Token
    {
        readonly get => (_bitfield >> 1) & 0x7FFFFFFFu;
        set => _bitfield = (_bitfield & ~(0x7FFFFFFFu << 1)) | ((value & 0x7FFFFFFFu) << 1);
    }
}

/// <remarks>
///     <see href="https://github.com/Unity-Technologies/mono/blob/unity-2017.1-mbe/mono/metadata/metadata.h#L301-L311"/>
/// </remarks>
internal unsafe struct MonoArrayType
{
    public MonoClass* EKlass;
    public byte Rank;
    public byte NumSizes;
    public byte NumLoBounds;
    public int* Sizes;
    public int* LoBounds;
}
