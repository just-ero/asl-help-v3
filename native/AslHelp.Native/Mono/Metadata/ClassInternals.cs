// Generated from Unity 2017.1 Mono headers using ClangSharpPInvokeGenerator.
//
// https://github.com/Unity-Technologies/mono
// https://github.com/dotnet/ClangSharp

using System.Runtime.InteropServices;

namespace AslHelp.Native.Mono.Metadata;

/// <remarks>
///     <see href="https://github.com/Unity-Technologies/mono/blob/unity-2017.1-mbe/mono/metadata/class-internals.h#L66-L96"/>
/// </remarks>
internal unsafe struct MonoMethod
{
    public ushort Flags;
    public ushort IFlags;
    public uint Token;
    public MonoClass* Klass;
    public MonoMethodSignature* Signature;
    public sbyte* Name;

    private uint _bitfield;

    public uint InlineInfo
    {
        readonly get => _bitfield & 0x1u;
        set => _bitfield = (_bitfield & ~0x1u) | (value & 0x1u);
    }

    public uint InlineFailure
    {
        readonly get => (_bitfield >> 1) & 0x1u;
        set => _bitfield = (_bitfield & ~(0x1u << 1)) | ((value & 0x1u) << 1);
    }

    public uint WrapperType
    {
        readonly get => (_bitfield >> 2) & 0x1Fu;
        set => _bitfield = (_bitfield & ~(0x1Fu << 2)) | ((value & 0x1Fu) << 2);
    }

    public uint StringCtor
    {
        readonly get => (_bitfield >> 7) & 0x1u;
        set => _bitfield = (_bitfield & ~(0x1u << 7)) | ((value & 0x1u) << 7);
    }

    public uint SaveLmf
    {
        readonly get => (_bitfield >> 8) & 0x1u;
        set => _bitfield = (_bitfield & ~(0x1u << 8)) | ((value & 0x1u) << 8);
    }

    public uint Dynamic
    {
        readonly get => (_bitfield >> 9) & 0x1u;
        set => _bitfield = (_bitfield & ~(0x1u << 9)) | ((value & 0x1u) << 9);
    }

    public uint SreMethod
    {
        readonly get => (_bitfield >> 10) & 0x1u;
        set => _bitfield = (_bitfield & ~(0x1u << 10)) | ((value & 0x1u) << 10);
    }

    public uint IsGeneric
    {
        readonly get => (_bitfield >> 11) & 0x1u;
        set => _bitfield = (_bitfield & ~(0x1u << 11)) | ((value & 0x1u) << 11);
    }

    public uint IsInflated
    {
        readonly get => (_bitfield >> 12) & 0x1u;
        set => _bitfield = (_bitfield & ~(0x1u << 12)) | ((value & 0x1u) << 12);
    }

    public uint SkipVisibility
    {
        readonly get => (_bitfield >> 13) & 0x1u;
        set => _bitfield = (_bitfield & ~(0x1u << 13)) | ((value & 0x1u) << 13);
    }

    public uint VerificationSuccess
    {
        readonly get => (_bitfield >> 14) & 0x1u;
        set => _bitfield = (_bitfield & ~(0x1u << 14)) | ((value & 0x1u) << 14);
    }

    public int Slot
    {
        readonly get => (int)(_bitfield << 1) >> 16;
        set => _bitfield = (_bitfield & ~(0xFFFFu << 15)) | (uint)((value & 0xFFFF) << 15);
    }
}

/// <remarks>
///     <see href="https://github.com/Unity-Technologies/mono/blob/unity-2017.1-mbe/mono/metadata/class-internals.h#L136-L153"/>
/// </remarks>
internal unsafe struct MonoClassField
{
    public MonoType* Type;
    public sbyte* Name;
    public MonoClass* Parent;
    public int Offset;
}

/// <remarks>
///     <see href="https://github.com/Unity-Technologies/mono/blob/unity-2017.1-mbe/mono/metadata/class-internals.h#L223-L227"/>
/// </remarks>
internal unsafe struct MonoClassRuntimeInfo
{
    public ushort MaxDomain;
    public MonoVTable* DomainVtables;
}

/// <remarks>
///     <see href="https://github.com/Unity-Technologies/mono/blob/unity-2017.1-mbe/mono/metadata/class-internals.h#L263-L415"/>
/// </remarks>
internal unsafe struct MonoClass
{
    public MonoClass* ElementClass;
    public MonoClass* CastClass;
    public MonoClass** SuperTypes;
    public ushort IDepth;
    public byte Rank;
    public int InstanceSize;

    private uint _bitfield1;

    public uint Inited
    {
        readonly get => _bitfield1 & 0x1u;
        set => _bitfield1 = (_bitfield1 & ~0x1u) | (value & 0x1u);
    }

    public uint InitPending
    {
        readonly get => (_bitfield1 >> 1) & 0x1u;
        set => _bitfield1 = (_bitfield1 & ~(0x1u << 1)) | ((value & 0x1u) << 1);
    }

    public uint SizeInited
    {
        readonly get => (_bitfield1 >> 2) & 0x1u;
        set => _bitfield1 = (_bitfield1 & ~(0x1u << 2)) | ((value & 0x1u) << 2);
    }

    public uint ValueType
    {
        readonly get => (_bitfield1 >> 3) & 0x1u;
        set => _bitfield1 = (_bitfield1 & ~(0x1u << 3)) | ((value & 0x1u) << 3);
    }

    public uint EnumType
    {
        readonly get => (_bitfield1 >> 4) & 0x1u;
        set => _bitfield1 = (_bitfield1 & ~(0x1u << 4)) | ((value & 0x1u) << 4);
    }

    public uint Blittable
    {
        readonly get => (_bitfield1 >> 5) & 0x1u;
        set => _bitfield1 = (_bitfield1 & ~(0x1u << 5)) | ((value & 0x1u) << 5);
    }

    public uint Unicode
    {
        readonly get => (_bitfield1 >> 6) & 0x1u;
        set => _bitfield1 = (_bitfield1 & ~(0x1u << 6)) | ((value & 0x1u) << 6);
    }

    public uint WasTypeBuilder
    {
        readonly get => (_bitfield1 >> 7) & 0x1u;
        set => _bitfield1 = (_bitfield1 & ~(0x1u << 7)) | ((value & 0x1u) << 7);
    }

    public byte MinAlign;

    private uint _bitfield2;

    public uint PackingSize
    {
        readonly get => _bitfield2 & 0xFu;
        set => _bitfield2 = (_bitfield2 & ~0xFu) | (value & 0xFu);
    }

    public uint GhcImpl
    {
        readonly get => (_bitfield2 >> 4) & 0x1u;
        set => _bitfield2 = (_bitfield2 & ~(0x1u << 4)) | ((value & 0x1u) << 4);
    }

    public uint HasFinalize
    {
        readonly get => (_bitfield2 >> 5) & 0x1u;
        set => _bitfield2 = (_bitfield2 & ~(0x1u << 5)) | ((value & 0x1u) << 5);
    }

    public uint MarshalByRef
    {
        readonly get => (_bitfield2 >> 6) & 0x1u;
        set => _bitfield2 = (_bitfield2 & ~(0x1u << 6)) | ((value & 0x1u) << 6);
    }

    public uint ContextBound
    {
        readonly get => (_bitfield2 >> 7) & 0x1u;
        set => _bitfield2 = (_bitfield2 & ~(0x1u << 7)) | ((value & 0x1u) << 7);
    }

    public uint Delegate
    {
        readonly get => (_bitfield2 >> 8) & 0x1u;
        set => _bitfield2 = (_bitfield2 & ~(0x1u << 8)) | ((value & 0x1u) << 8);
    }

    public uint GcDescrInited
    {
        readonly get => (_bitfield2 >> 9) & 0x1u;
        set => _bitfield2 = (_bitfield2 & ~(0x1u << 9)) | ((value & 0x1u) << 9);
    }

    public uint HasCctor
    {
        readonly get => (_bitfield2 >> 10) & 0x1u;
        set => _bitfield2 = (_bitfield2 & ~(0x1u << 10)) | ((value & 0x1u) << 10);
    }

    public uint HasReferences
    {
        readonly get => (_bitfield2 >> 11) & 0x1u;
        set => _bitfield2 = (_bitfield2 & ~(0x1u << 11)) | ((value & 0x1u) << 11);
    }

    public uint HasStaticRefs
    {
        readonly get => (_bitfield2 >> 12) & 0x1u;
        set => _bitfield2 = (_bitfield2 & ~(0x1u << 12)) | ((value & 0x1u) << 12);
    }

    public uint NoSpecialStaticFields
    {
        readonly get => (_bitfield2 >> 13) & 0x1u;
        set => _bitfield2 = (_bitfield2 & ~(0x1u << 13)) | ((value & 0x1u) << 13);
    }

    public uint IsComObject
    {
        readonly get => (_bitfield2 >> 14) & 0x1u;
        set => _bitfield2 = (_bitfield2 & ~(0x1u << 14)) | ((value & 0x1u) << 14);
    }

    public uint NestedClassesInited
    {
        readonly get => (_bitfield2 >> 15) & 0x1u;
        set => _bitfield2 = (_bitfield2 & ~(0x1u << 15)) | ((value & 0x1u) << 15);
    }

    public uint InterfacesInited
    {
        readonly get => (_bitfield2 >> 16) & 0x1u;
        set => _bitfield2 = (_bitfield2 & ~(0x1u << 16)) | ((value & 0x1u) << 16);
    }

    public uint SimdType
    {
        readonly get => (_bitfield2 >> 17) & 0x1u;
        set => _bitfield2 = (_bitfield2 & ~(0x1u << 17)) | ((value & 0x1u) << 17);
    }

    public uint IsGeneric
    {
        readonly get => (_bitfield2 >> 18) & 0x1u;
        set => _bitfield2 = (_bitfield2 & ~(0x1u << 18)) | ((value & 0x1u) << 18);
    }

    public uint IsInflated
    {
        readonly get => (_bitfield2 >> 19) & 0x1u;
        set => _bitfield2 = (_bitfield2 & ~(0x1u << 19)) | ((value & 0x1u) << 19);
    }

    public uint HasFinalizeInited
    {
        readonly get => (_bitfield2 >> 20) & 0x1u;
        set => _bitfield2 = (_bitfield2 & ~(0x1u << 20)) | ((value & 0x1u) << 20);
    }

    public uint FieldsInited
    {
        readonly get => (_bitfield2 >> 21) & 0x1u;
        set => _bitfield2 = (_bitfield2 & ~(0x1u << 21)) | ((value & 0x1u) << 21);
    }

    public uint SetupFieldsCalled
    {
        readonly get => (_bitfield2 >> 22) & 0x1u;
        set => _bitfield2 = (_bitfield2 & ~(0x1u << 22)) | ((value & 0x1u) << 22);
    }

    public byte ExceptionType;
    public MonoClass* Parent;
    public MonoClass* NestedIn;
    public MonoImage* Image;
    public sbyte* Name;
    public sbyte* NameSpace;
    public uint TypeToken;
    public int VtableSize;
    public ushort InterfaceCount;
    public ushort InterfaceId;
    public ushort MaxInterfaceId;
    public ushort InterfaceOffsetsCount;
    public MonoClass** InterfacesPacked;
    public ushort* InterfaceOffsetsPacked;
    public byte* InterfaceBitmap;
    public MonoClass** Interfaces;
    public SizesUnion Sizes;
    public uint Flags;
    public (uint First, uint Count) Field;
    public (uint First, uint Count) Method;
    public uint RefInfoHandle;
    public void* MarshalInfo;
    public MonoClassField* Fields;
    public MonoMethod** Methods;
    public MonoType ThisArg;
    public MonoType ByvalArg;
    public MonoGenericClass* GenericClass;
    public MonoGenericContainer* GenericContainer;
    public void* GcDescr;
    public MonoClassRuntimeInfo* RuntimeInfo;
    public MonoClass* NextClassCache;
    public MonoMethod** Vtable;
    public void* Ext;
    public void* UnityUserData;

    [StructLayout(LayoutKind.Explicit)]
    public struct SizesUnion
    {
        [FieldOffset(0)] public int ClassSize;
        [FieldOffset(0)] public int ElementSize;
        [FieldOffset(0)] public int GenericParamToken;
    }
}

/// <remarks>
///     <see href="https://github.com/Unity-Technologies/mono/blob/unity-2017.1-mbe/mono/metadata/class-internals.h#L455-L480"/>
/// </remarks>
internal unsafe struct MonoVTable
{
    public MonoClass* Klass;
    public void* GcDescr;
    public void* Domain;
    public void* Type;
    public byte* InterfaceBitmap;
    public ushort MaxInterfaceId;
    public byte Rank;
    public byte Initialized;

    private uint _bitfield;

    public uint Remote
    {
        readonly get => _bitfield & 0x1u;
        set => _bitfield = (_bitfield & ~0x1u) | (value & 0x1u);
    }

    public uint InitFailed
    {
        readonly get => (_bitfield >> 1) & 0x1u;
        set => _bitfield = (_bitfield & ~(0x1u << 1)) | ((value & 0x1u) << 1);
    }

    public uint HasStaticFields
    {
        readonly get => (_bitfield >> 2) & 0x1u;
        set => _bitfield = (_bitfield & ~(0x1u << 2)) | ((value & 0x1u) << 2);
    }

    public uint GcBits
    {
        readonly get => (_bitfield >> 3) & 0xFu;
        set => _bitfield = (_bitfield & ~(0xFu << 3)) | ((value & 0xFu) << 3);
    }

    public uint ImtCollisionsBitmap;
    public void** RuntimeGenericContext;
    public void* Vtable;
}

/// <remarks>
///     <see href="https://github.com/Unity-Technologies/mono/blob/unity-2017.1-mbe/mono/metadata/class-internals.h#L496-L503"/>
/// </remarks>
internal unsafe struct MonoGenericInst
{
    public uint Id;

    private uint _bitfield;

    public uint TypeArgc
    {
        readonly get => _bitfield & 0x3FFFFFu;
        set => _bitfield = (_bitfield & ~0x3FFFFFu) | (value & 0x3FFFFFu);
    }

    public uint IsOpen
    {
        readonly get => (_bitfield >> 22) & 0x1u;
        set => _bitfield = (_bitfield & ~(0x1u << 22)) | ((value & 0x1u) << 22);
    }

    public TypeArgvBuffer TypeArgv;

    public readonly unsafe struct TypeArgvBuffer
    {
        private readonly MonoType* _e0;

        public ref MonoType* this[int index]
        {
            get
            {
                fixed (MonoType** pThis = &_e0)
                {
                    return ref pThis[index];
                }
            }
        }
    }
}

/// <remarks>
///     <see href="https://github.com/Unity-Technologies/mono/blob/unity-2017.1-mbe/mono/metadata/class-internals.h#L513-L518"/>
/// </remarks>
internal unsafe struct MonoGenericContext
{
    public MonoGenericInst* ClassInst;
    public MonoGenericInst* MethodInst;
}

/// <remarks>
///     <see href="https://github.com/Unity-Technologies/mono/blob/unity-2017.1-mbe/mono/metadata/class-internals.h#L536-L550"/>
/// </remarks>
internal unsafe struct MonoGenericClass
{
    public MonoClass* ContainerClass;
    public MonoGenericContext Context;

    private uint _bitfield;

    public uint IsDynamic
    {
        readonly get => _bitfield & 0x1u;
        set => _bitfield = (_bitfield & ~0x1u) | (value & 0x1u);
    }

    public uint IsTbOpen
    {
        readonly get => (_bitfield >> 1) & 0x1u;
        set => _bitfield = (_bitfield & ~(0x1u << 1)) | ((value & 0x1u) << 1);
    }

    public uint NeedSync
    {
        readonly get => (_bitfield >> 2) & 0x1u;
        set => _bitfield = (_bitfield & ~(0x1u << 2)) | ((value & 0x1u) << 2);
    }

    public MonoClass* CachedClass;
}

/// <remarks>
///     <see href="https://github.com/Unity-Technologies/mono/blob/unity-2017.1-mbe/mono/metadata/class-internals.h#L555-L566"/>
/// </remarks>
internal unsafe struct MonoGenericParam
{
    public MonoGenericContainer* Owner;
    public ushort Num;
    public MonoType* GSharedConstraint;
}

/// <remarks>
///     <see href="https://github.com/Unity-Technologies/mono/blob/unity-2017.1-mbe/mono/metadata/class-internals.h#L570-L581"/>
/// </remarks>
internal unsafe struct MonoGenericParamInfo
{
    public MonoClass* PKlass;
    public sbyte* Name;
    public ushort Flags;
    public uint Token;
    public MonoClass** Constraints;
}

/// <remarks>
///     <see href="https://github.com/Unity-Technologies/mono/blob/unity-2017.1-mbe/mono/metadata/class-internals.h#L583-L586"/>
/// </remarks>
internal unsafe struct MonoGenericParamFull
{
    public MonoGenericParam Param;
    public MonoGenericParamInfo Info;
}

/// <remarks>
///     <see href="https://github.com/Unity-Technologies/mono/blob/unity-2017.1-mbe/mono/metadata/class-internals.h#L593-L621"/>
/// </remarks>
internal unsafe struct MonoGenericContainer
{
    public MonoGenericContext Context;
    public MonoGenericContainer* Parent;
    public OwnerUnion Owner;

    private int _bitfield;

    public int TypeArgc
    {
        readonly get => (_bitfield << 3) >> 3;
        set => _bitfield = (_bitfield & ~0x1FFFFFFF) | (value & 0x1FFFFFFF);
    }

    public int IsMethod
    {
        readonly get => (_bitfield << 2) >> 31;
        set => _bitfield = (_bitfield & ~(0x1 << 29)) | ((value & 0x1) << 29);
    }

    public int IsAnonymous
    {
        readonly get => (_bitfield << 1) >> 31;
        set => _bitfield = (_bitfield & ~(0x1 << 30)) | ((value & 0x1) << 30);
    }

    public int IsSmallParam
    {
        readonly get => (_bitfield << 0) >> 31;
        set => _bitfield = (_bitfield & ~(0x1 << 31)) | ((value & 0x1) << 31);
    }

    public MonoGenericParamFull* TypeParams;

    [StructLayout(LayoutKind.Explicit)]
    public struct OwnerUnion
    {
        [FieldOffset(0)] public MonoClass* Klass;
        [FieldOffset(0)] public MonoMethod* Method;
        [FieldOffset(0)] public MonoImage* Image;
    }
}
