// Generated from Unity 2017.1 Mono headers using ClangSharpPInvokeGenerator.
//
// https://github.com/Unity-Technologies/mono
// https://github.com/dotnet/ClangSharp

using System.Runtime.CompilerServices;

using AslHelp.Native.Eglib;
using AslHelp.Native.Mono.Utils;

namespace AslHelp.Native.Mono.Metadata;

/// <remarks>
///     <see href="https://github.com/Unity-Technologies/mono/blob/unity-2017.1-mbe/mono/metadata/metadata-internals.h#L176-L407"/>
/// </remarks>
internal unsafe partial struct MonoImage
{
    public int RefCount;
    public void* RawDataHandle;
    public sbyte* RawData;
    public uint RawDataLen;

    private byte _bitfield0;

    public byte RawBufferUsed
    {
        readonly get => (byte)(_bitfield0 & 0x1u);
        set => _bitfield0 = (byte)((_bitfield0 & ~0x1u) | (value & 0x1u));
    }

    public byte RawDataAllocated
    {
        readonly get => (byte)((_bitfield0 >> 1) & 0x1u);
        set => _bitfield0 = (byte)((_bitfield0 & ~(0x1u << 1)) | ((value & 0x1u) << 1));
    }

    public byte FileioUsed
    {
        readonly get => (byte)((_bitfield0 >> 2) & 0x1u);
        set => _bitfield0 = (byte)((_bitfield0 & ~(0x1u << 2)) | ((value & 0x1u) << 2));
    }

    public byte Dynamic
    {
        readonly get => (byte)((_bitfield0 >> 3) & 0x1u);
        set => _bitfield0 = (byte)((_bitfield0 & ~(0x1u << 3)) | ((value & 0x1u) << 3));
    }

    public byte RefOnly
    {
        readonly get => (byte)((_bitfield0 >> 4) & 0x1u);
        set => _bitfield0 = (byte)((_bitfield0 & ~(0x1u << 4)) | ((value & 0x1u) << 4));
    }

    public byte UncompressedMetadata
    {
        readonly get => (byte)((_bitfield0 >> 5) & 0x1u);
        set => _bitfield0 = (byte)((_bitfield0 & ~(0x1u << 5)) | ((value & 0x1u) << 5));
    }

    public byte MetadataOnly
    {
        readonly get => (byte)((_bitfield0 >> 6) & 0x1u);
        set => _bitfield0 = (byte)((_bitfield0 & ~(0x1u << 6)) | ((value & 0x1u) << 6));
    }

    public byte CheckedModuleCctor
    {
        readonly get => (byte)((_bitfield0 >> 7) & 0x1u);

        set => _bitfield0 = (byte)((_bitfield0 & ~(0x1u << 7)) | ((value & 0x1u) << 7));
    }

    private byte _bitfield1;

    public byte HasModuleCctor
    {
        readonly get => (byte)(_bitfield1 & 0x1u);
        set => _bitfield1 = (byte)((_bitfield1 & ~0x1u) | (value & 0x1u));
    }

    public byte IdxStringWide
    {
        readonly get => (byte)((_bitfield1 >> 1) & 0x1u);
        set => _bitfield1 = (byte)((_bitfield1 & ~(0x1u << 1)) | ((value & 0x1u) << 1));
    }

    public byte IdxGuidWide
    {
        readonly get => (byte)((_bitfield1 >> 2) & 0x1u);
        set => _bitfield1 = (byte)((_bitfield1 & ~(0x1u << 2)) | ((value & 0x1u) << 2));
    }

    public byte IdxBlobWide
    {
        readonly get => (byte)((_bitfield1 >> 3) & 0x1u);
        set => _bitfield1 = (byte)((_bitfield1 & ~(0x1u << 3)) | ((value & 0x1u) << 3));
    }

    public byte CoreClrPlatformCode
    {
        readonly get => (byte)((_bitfield1 >> 4) & 0x1u);
        set => _bitfield1 = (byte)((_bitfield1 & ~(0x1u << 4)) | ((value & 0x1u) << 4));
    }

    public sbyte* Name;
    public sbyte* AssemblyName;
    public sbyte* ModuleName;
    public sbyte* Version;
    public short MdVersionMajor;
    public short MdVersionMinor;
    public sbyte* Guid;
    public void* ImageInfo;
    public MonoMemPool* Mempool;
    public sbyte* RawMetadata;
    public MonoStreamHeader HeapStrings;
    public MonoStreamHeader HeapUs;
    public MonoStreamHeader HeapBlob;
    public MonoStreamHeader HeapGuid;
    public MonoStreamHeader HeapTables;
    public MonoStreamHeader HeapPdb;
    public sbyte* TablesBase;
    public TablesBuffer Tables;
    public MonoAssembly** References;
    public int NReferences;
    public MonoImage** Modules;
    public uint ModuleCount;
    public int* ModulesLoaded;
    public MonoImage** Files;
    public void* AotModule;
    public fixed byte AotId[16];
    public MonoAssembly* Assembly;
    public GHashTable* MethodCache;
    public MonoInternalHashTable ClassCache;

    [InlineArray((int)MonoMetaTable.Num)]
    public struct TablesBuffer
    {
    }
}

/// <remarks>
///     <see href="https://github.com/Unity-Technologies/mono/blob/unity-2017.1-mbe/mono/metadata/metadata-internals.h#L142-L145"/>
/// </remarks>
internal unsafe struct MonoStreamHeader
{
    public sbyte* Data;
    public uint Size;
}

/// <remarks>
///     <see href="https://github.com/Unity-Technologies/mono/blob/unity-2017.1-mbe/mono/metadata/metadata-internals.h#L147-L163"/>
/// </remarks>
internal unsafe struct MonoTableInfo
{
    public sbyte* Base;

    private uint _bitfield;

    public uint Rows
    {
        readonly get => _bitfield & 0xFFFFFFu;
        set => _bitfield = (_bitfield & ~0xFFFFFFu) | (value & 0xFFFFFFu);
    }

    public uint RowSize
    {
        readonly get => (_bitfield >> 24) & 0xFFu;
        set => _bitfield = (_bitfield & ~(0xFFu << 24)) | ((value & 0xFFu) << 24);
    }

    public uint SizeBitfield;
}

/// <remarks>
///     <see href="https://github.com/Unity-Technologies/mono/blob/unity-2017.1-mbe/mono/metadata/metadata-internals.h#L73-L102"/>
/// </remarks>
internal unsafe struct MonoAssembly
{
    public int RefCount;
    public sbyte* BaseDir;
    public MonoAssemblyName AName;
    public MonoImage* Image;
    public GSList* FriendAssemblyNames;
    public byte FriendAssemblyNamesInited;
    public byte InGac;
    public byte Dynamic;
    public byte CorlibInternal;
    public int RefOnly;
    public byte WrapNonExceptionThrows;
    public byte WrapNonExceptionThrowsInited;
    public byte JitOptimizerDisabled;
    public byte JitOptimizerDisabledInited;

    private uint _bitfield;

    public uint ECMA
    {
        readonly get => _bitfield & 0x3u;
        set => _bitfield = (_bitfield & ~0x3u) | (value & 0x3u);
    }

    public uint AllowPartiallyTrustedCallers
    {
        readonly get => (_bitfield >> 2) & 0x3u;
        set => _bitfield = (_bitfield & ~(0x3u << 2)) | ((value & 0x3u) << 2);
    }

    public uint FullTrust
    {
        readonly get => (_bitfield >> 4) & 0x3u;
        set => _bitfield = (_bitfield & ~(0x3u << 4)) | ((value & 0x3u) << 4);
    }

    public uint Unmanaged
    {
        readonly get => (_bitfield >> 6) & 0x3u;
        set => _bitfield = (_bitfield & ~(0x3u << 6)) | ((value & 0x3u) << 6);
    }

    public uint SkipVerification
    {
        readonly get => (_bitfield >> 8) & 0x3u;
        set => _bitfield = (_bitfield & ~(0x3u << 8)) | ((value & 0x3u) << 8);
    }
}

/// <remarks>
///     <see href="https://github.com/Unity-Technologies/mono/blob/unity-2017.1-mbe/mono/metadata/metadata-internals.h#L51-L62"/>
/// </remarks>
internal unsafe struct MonoAssemblyName
{
    public sbyte* Name;
    public sbyte* Culture;
    public sbyte* HashValue;
    public byte* PublicKey;
    public fixed byte PublicKeyToken[17];
    public uint HashAlg;
    public uint HashLen;
    public uint Flags;
    public ushort Major;
    public ushort Minor;
    public ushort Build;
    public ushort Revision;
    public ushort Arch;
}
