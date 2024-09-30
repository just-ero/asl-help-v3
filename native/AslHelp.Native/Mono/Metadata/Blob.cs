// Generated from Unity 2017.1 Mono headers using ClangSharpPInvokeGenerator.
//
// https://github.com/Unity-Technologies/mono
// https://github.com/dotnet/ClangSharp

namespace AslHelp.Native.Mono.Metadata;

/// <remarks>
///     <see href="https://github.com/Unity-Technologies/mono/blob/unity-2017.1-mbe/mono/metadata/blob.h#L11-L50"/>
/// </remarks>
internal enum MonoTypeEnum
{
    End = 0x00,
    Void = 0x01,
    Boolean = 0x02,
    Char = 0x03,
    I1 = 0x04,
    U1 = 0x05,
    I2 = 0x06,
    U2 = 0x07,
    I4 = 0x08,
    U4 = 0x09,
    I8 = 0x0a,
    U8 = 0x0b,
    R4 = 0x0c,
    R8 = 0x0d,
    String = 0x0e,
    Ptr = 0x0f,
    ByRef = 0x10,
    ValueType = 0x11,
    Class = 0x12,
    Var = 0x13,
    Array = 0x14,
    GenericInst = 0x15,
    TypedByRef = 0x16,
    I = 0x18,
    U = 0x19,
    FnPtr = 0x1b,
    Object = 0x1c,
    SzArray = 0x1d,
    MVar = 0x1e,
    CmodReqd = 0x1f,
    CmodOpt = 0x20,
    Internal = 0x21,
    Modifier = 0x40,
    Sentinel = 0x41,
    Pinned = 0x45,
    Enum = 0x55
}

/// <remarks>
///     <see href="https://github.com/Unity-Technologies/mono/blob/unity-2017.1-mbe/mono/metadata/blob.h#L52-L114"/>
/// </remarks>
internal enum MonoMetaTable
{
    Module,
    TypeRef,
    TypeDef,
    FieldPointer,
    Field,
    MethodPointer,
    Method,
    ParamPointer,
    Param,
    InterfaceImpl,
    MemberRef,
    Constant,
    CustomAttribute,
    FieldMarshal,
    DeclSecurity,
    ClassLayout,
    FieldLayout,
    StandaloneSig,
    EventMap,
    EventPointer,
    Event,
    PropertyMap,
    PropertyPointer,
    Property,
    MethodSemantics,
    MethodImpl,
    ModuleRef,
    TypeSpec,
    ImplMap,
    FieldRva,
    Unused6,
    Unused7,
    Assembly,
    AssemblyProcessor,
    AssemblyOs,
    AssemblyRef,
    AssemblyRefProcessor,
    AssemblyRefOs,
    File,
    ExportedType,
    ManifestResource,
    NestedClass,
    GenericParam,
    MethodSpec,
    GenericParamConstraint,
    Unused8,
    Unused9,
    Unused10,
    Document,
    MethodBody,
    LocalScope,
    LocalVariable,
    LocalConstant,
    ImportScope,
    AsyncMethod,
    CustomDebugInformation,

    Last = CustomDebugInformation,
    Num = Last + 1
}
