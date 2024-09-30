using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace ClangSharp.Generated
{
    public unsafe partial struct MonoMethod
    {
        [NativeTypeName("guint16")]
        public ushort flags;

        [NativeTypeName("guint16")]
        public ushort iflags;

        [NativeTypeName("guint32")]
        public uint token;

        [NativeTypeName("struct MonoClass *")]
        public MonoClass* klass;

        [NativeTypeName("struct MonoMethodSignature *")]
        public MonoMethodSignature* signature;

        [NativeTypeName("const char *")]
        public sbyte* name;

        public uint _bitfield;

        [NativeTypeName("unsigned int : 1")]
        public uint inline_info
        {
            readonly get
            {
                return _bitfield & 0x1u;
            }

            set
            {
                _bitfield = (_bitfield & ~0x1u) | (value & 0x1u);
            }
        }

        [NativeTypeName("unsigned int : 1")]
        public uint inline_failure
        {
            readonly get
            {
                return (_bitfield >> 1) & 0x1u;
            }

            set
            {
                _bitfield = (_bitfield & ~(0x1u << 1)) | ((value & 0x1u) << 1);
            }
        }

        [NativeTypeName("unsigned int : 5")]
        public uint wrapper_type
        {
            readonly get
            {
                return (_bitfield >> 2) & 0x1Fu;
            }

            set
            {
                _bitfield = (_bitfield & ~(0x1Fu << 2)) | ((value & 0x1Fu) << 2);
            }
        }

        [NativeTypeName("unsigned int : 1")]
        public uint string_ctor
        {
            readonly get
            {
                return (_bitfield >> 7) & 0x1u;
            }

            set
            {
                _bitfield = (_bitfield & ~(0x1u << 7)) | ((value & 0x1u) << 7);
            }
        }

        [NativeTypeName("unsigned int : 1")]
        public uint save_lmf
        {
            readonly get
            {
                return (_bitfield >> 8) & 0x1u;
            }

            set
            {
                _bitfield = (_bitfield & ~(0x1u << 8)) | ((value & 0x1u) << 8);
            }
        }

        [NativeTypeName("unsigned int : 1")]
        public uint dynamic
        {
            readonly get
            {
                return (_bitfield >> 9) & 0x1u;
            }

            set
            {
                _bitfield = (_bitfield & ~(0x1u << 9)) | ((value & 0x1u) << 9);
            }
        }

        [NativeTypeName("unsigned int : 1")]
        public uint sre_method
        {
            readonly get
            {
                return (_bitfield >> 10) & 0x1u;
            }

            set
            {
                _bitfield = (_bitfield & ~(0x1u << 10)) | ((value & 0x1u) << 10);
            }
        }

        [NativeTypeName("unsigned int : 1")]
        public uint is_generic
        {
            readonly get
            {
                return (_bitfield >> 11) & 0x1u;
            }

            set
            {
                _bitfield = (_bitfield & ~(0x1u << 11)) | ((value & 0x1u) << 11);
            }
        }

        [NativeTypeName("unsigned int : 1")]
        public uint is_inflated
        {
            readonly get
            {
                return (_bitfield >> 12) & 0x1u;
            }

            set
            {
                _bitfield = (_bitfield & ~(0x1u << 12)) | ((value & 0x1u) << 12);
            }
        }

        [NativeTypeName("unsigned int : 1")]
        public uint skip_visibility
        {
            readonly get
            {
                return (_bitfield >> 13) & 0x1u;
            }

            set
            {
                _bitfield = (_bitfield & ~(0x1u << 13)) | ((value & 0x1u) << 13);
            }
        }

        [NativeTypeName("unsigned int : 1")]
        public uint verification_success
        {
            readonly get
            {
                return (_bitfield >> 14) & 0x1u;
            }

            set
            {
                _bitfield = (_bitfield & ~(0x1u << 14)) | ((value & 0x1u) << 14);
            }
        }

        [NativeTypeName("int : 16")]
        public int slot
        {
            readonly get
            {
                return (int)(_bitfield << 1) >> 16;
            }

            set
            {
                _bitfield = (_bitfield & ~(0xFFFFu << 15)) | (uint)((value & 0xFFFF) << 15);
            }
        }
    }

    public partial struct MonoGenericInst
    {
        [NativeTypeName("guint")]
        public uint id;

        public uint _bitfield;

        [NativeTypeName("guint : 22")]
        public uint type_argc
        {
            readonly get
            {
                return _bitfield & 0x3FFFFFu;
            }

            set
            {
                _bitfield = (_bitfield & ~0x3FFFFFu) | (value & 0x3FFFFFu);
            }
        }

        [NativeTypeName("guint : 1")]
        public uint is_open
        {
            readonly get
            {
                return (_bitfield >> 22) & 0x1u;
            }

            set
            {
                _bitfield = (_bitfield & ~(0x1u << 22)) | ((value & 0x1u) << 22);
            }
        }

        [NativeTypeName("struct MonoType *[1]")]
        public _type_argv_e__FixedBuffer type_argv;

        public unsafe partial struct _type_argv_e__FixedBuffer
        {
            public MonoType* e0;

            public ref MonoType* this[int index]
            {
                get
                {
                    fixed (MonoType** pThis = &e0)
                    {
                        return ref pThis[index];
                    }
                }
            }
        }
    }

    public unsafe partial struct MonoGenericContext
    {
        public MonoGenericInst* class_inst;

        public MonoGenericInst* method_inst;
    }

    public unsafe partial struct MonoGenericContainer
    {
        public MonoGenericContext context;

        public MonoGenericContainer* parent;

        [NativeTypeName("__AnonymousRecord_input_L90_C3")]
        public _owner_e__Union owner;

        public int _bitfield;

        [NativeTypeName("int : 29")]
        public int type_argc
        {
            readonly get
            {
                return (_bitfield << 3) >> 3;
            }

            set
            {
                _bitfield = (_bitfield & ~0x1FFFFFFF) | (value & 0x1FFFFFFF);
            }
        }

        [NativeTypeName("int : 1")]
        public int is_method
        {
            readonly get
            {
                return (_bitfield << 2) >> 31;
            }

            set
            {
                _bitfield = (_bitfield & ~(0x1 << 29)) | ((value & 0x1) << 29);
            }
        }

        [NativeTypeName("int : 1")]
        public int is_anonymous
        {
            readonly get
            {
                return (_bitfield << 1) >> 31;
            }

            set
            {
                _bitfield = (_bitfield & ~(0x1 << 30)) | ((value & 0x1) << 30);
            }
        }

        [NativeTypeName("int : 1")]
        public int is_small_param
        {
            readonly get
            {
                return (_bitfield << 0) >> 31;
            }

            set
            {
                _bitfield = (_bitfield & ~(0x1 << 31)) | ((value & 0x1) << 31);
            }
        }

        public void* type_params;

        [StructLayout(LayoutKind.Explicit)]
        public unsafe partial struct _owner_e__Union
        {
            [FieldOffset(0)]
            [NativeTypeName("struct MonoClass *")]
            public MonoClass* klass;

            [FieldOffset(0)]
            public MonoMethod* method;

            [FieldOffset(0)]
            public void* image;
        }
    }

    public unsafe partial struct MonoGenericParam
    {
        public MonoGenericContainer* owner;

        [NativeTypeName("guint16")]
        public ushort num;

        [NativeTypeName("struct MonoType *")]
        public MonoType* gshared_constraint;
    }

    public unsafe partial struct MonoGenericParamInfo
    {
        [NativeTypeName("struct MonoClass *")]
        public MonoClass* pklass;

        [NativeTypeName("const char *")]
        public sbyte* name;

        [NativeTypeName("guint16")]
        public ushort flags;

        [NativeTypeName("guint32")]
        public uint token;

        [NativeTypeName("struct MonoClass **")]
        public MonoClass** constraints;
    }

    public partial struct MonoGenericParamFull
    {
        public MonoGenericParam param;

        public MonoGenericParamInfo info;
    }

    public unsafe partial struct MonoMethodSignature
    {
        [NativeTypeName("struct MonoType *")]
        public MonoType* ret;

        [NativeTypeName("guint16")]
        public ushort param_count;

        [NativeTypeName("gint16")]
        public short sentinelpos;

        public uint _bitfield;

        [NativeTypeName("unsigned int : 16")]
        public uint generic_param_count
        {
            readonly get
            {
                return _bitfield & 0xFFFFu;
            }

            set
            {
                _bitfield = (_bitfield & ~0xFFFFu) | (value & 0xFFFFu);
            }
        }

        [NativeTypeName("unsigned int : 6")]
        public uint call_convention
        {
            readonly get
            {
                return (_bitfield >> 16) & 0x3Fu;
            }

            set
            {
                _bitfield = (_bitfield & ~(0x3Fu << 16)) | ((value & 0x3Fu) << 16);
            }
        }

        [NativeTypeName("unsigned int : 1")]
        public uint hasthis
        {
            readonly get
            {
                return (_bitfield >> 22) & 0x1u;
            }

            set
            {
                _bitfield = (_bitfield & ~(0x1u << 22)) | ((value & 0x1u) << 22);
            }
        }

        [NativeTypeName("unsigned int : 1")]
        public uint explicit_this
        {
            readonly get
            {
                return (_bitfield >> 23) & 0x1u;
            }

            set
            {
                _bitfield = (_bitfield & ~(0x1u << 23)) | ((value & 0x1u) << 23);
            }
        }

        [NativeTypeName("unsigned int : 1")]
        public uint pinvoke
        {
            readonly get
            {
                return (_bitfield >> 24) & 0x1u;
            }

            set
            {
                _bitfield = (_bitfield & ~(0x1u << 24)) | ((value & 0x1u) << 24);
            }
        }

        [NativeTypeName("unsigned int : 1")]
        public uint is_inflated
        {
            readonly get
            {
                return (_bitfield >> 25) & 0x1u;
            }

            set
            {
                _bitfield = (_bitfield & ~(0x1u << 25)) | ((value & 0x1u) << 25);
            }
        }

        [NativeTypeName("unsigned int : 1")]
        public uint has_type_parameters
        {
            readonly get
            {
                return (_bitfield >> 26) & 0x1u;
            }

            set
            {
                _bitfield = (_bitfield & ~(0x1u << 26)) | ((value & 0x1u) << 26);
            }
        }

        [NativeTypeName("struct MonoType *[1]")]
        public _params_e__FixedBuffer @params;

        public unsafe partial struct _params_e__FixedBuffer
        {
            public MonoType* e0;

            public ref MonoType* this[int index]
            {
                get
                {
                    fixed (MonoType** pThis = &e0)
                    {
                        return ref pThis[index];
                    }
                }
            }
        }
    }

    public unsafe partial struct MonoArrayType
    {
        [NativeTypeName("struct MonoClass *")]
        public MonoClass* eklass;

        [NativeTypeName("uint8_t")]
        public byte rank;

        [NativeTypeName("uint8_t")]
        public byte numsizes;

        [NativeTypeName("uint8_t")]
        public byte numlobounds;

        public int* sizes;

        public int* lobounds;
    }

    public unsafe partial struct MonoGenericClass
    {
        [NativeTypeName("struct MonoClass *")]
        public MonoClass* container_class;

        public MonoGenericContext context;

        public uint _bitfield;

        [NativeTypeName("guint : 1")]
        public uint is_dynamic
        {
            readonly get
            {
                return _bitfield & 0x1u;
            }

            set
            {
                _bitfield = (_bitfield & ~0x1u) | (value & 0x1u);
            }
        }

        [NativeTypeName("guint : 1")]
        public uint is_tb_open
        {
            readonly get
            {
                return (_bitfield >> 1) & 0x1u;
            }

            set
            {
                _bitfield = (_bitfield & ~(0x1u << 1)) | ((value & 0x1u) << 1);
            }
        }

        [NativeTypeName("guint : 1")]
        public uint need_sync
        {
            readonly get
            {
                return (_bitfield >> 2) & 0x1u;
            }

            set
            {
                _bitfield = (_bitfield & ~(0x1u << 2)) | ((value & 0x1u) << 2);
            }
        }

        [NativeTypeName("struct MonoClass *")]
        public MonoClass* cached_class;
    }

    public enum MonoTypeEnum
    {
        MONO_TYPE_END = 0x00,
        MONO_TYPE_VOID = 0x01,
        MONO_TYPE_BOOLEAN = 0x02,
        MONO_TYPE_CHAR = 0x03,
        MONO_TYPE_I1 = 0x04,
        MONO_TYPE_U1 = 0x05,
        MONO_TYPE_I2 = 0x06,
        MONO_TYPE_U2 = 0x07,
        MONO_TYPE_I4 = 0x08,
        MONO_TYPE_U4 = 0x09,
        MONO_TYPE_I8 = 0x0a,
        MONO_TYPE_U8 = 0x0b,
        MONO_TYPE_R4 = 0x0c,
        MONO_TYPE_R8 = 0x0d,
        MONO_TYPE_STRING = 0x0e,
        MONO_TYPE_PTR = 0x0f,
        MONO_TYPE_BYREF = 0x10,
        MONO_TYPE_VALUETYPE = 0x11,
        MONO_TYPE_CLASS = 0x12,
        MONO_TYPE_VAR = 0x13,
        MONO_TYPE_ARRAY = 0x14,
        MONO_TYPE_GENERICINST = 0x15,
        MONO_TYPE_TYPEDBYREF = 0x16,
        MONO_TYPE_I = 0x18,
        MONO_TYPE_U = 0x19,
        MONO_TYPE_FNPTR = 0x1b,
        MONO_TYPE_OBJECT = 0x1c,
        MONO_TYPE_SZARRAY = 0x1d,
        MONO_TYPE_MVAR = 0x1e,
        MONO_TYPE_CMOD_REQD = 0x1f,
        MONO_TYPE_CMOD_OPT = 0x20,
        MONO_TYPE_INTERNAL = 0x21,
        MONO_TYPE_MODIFIER = 0x40,
        MONO_TYPE_SENTINEL = 0x41,
        MONO_TYPE_PINNED = 0x45,
        MONO_TYPE_ENUM = 0x55,
    }

    public partial struct MonoCustomMod
    {
        public uint _bitfield;

        [NativeTypeName("unsigned int : 1")]
        public uint required
        {
            readonly get
            {
                return _bitfield & 0x1u;
            }

            set
            {
                _bitfield = (_bitfield & ~0x1u) | (value & 0x1u);
            }
        }

        [NativeTypeName("unsigned int : 31")]
        public uint token
        {
            readonly get
            {
                return (_bitfield >> 1) & 0x7FFFFFFFu;
            }

            set
            {
                _bitfield = (_bitfield & ~(0x7FFFFFFFu << 1)) | ((value & 0x7FFFFFFFu) << 1);
            }
        }
    }

    public partial struct MonoType
    {
        [NativeTypeName("__AnonymousRecord_input_L242_C3")]
        public _data_e__Union data;

        public uint _bitfield;

        [NativeTypeName("unsigned int : 16")]
        public uint attrs
        {
            readonly get
            {
                return _bitfield & 0xFFFFu;
            }

            set
            {
                _bitfield = (_bitfield & ~0xFFFFu) | (value & 0xFFFFu);
            }
        }

        [NativeTypeName("MonoTypeEnum : 8")]
        public MonoTypeEnum type
        {
            readonly get
            {
                return (MonoTypeEnum)(_bitfield << 8) >> 24;
            }

            set
            {
                _bitfield = (_bitfield & ~(0xFFu << 16)) | (((uint)(value) & 0xFF) << 16);
            }
        }

        [NativeTypeName("unsigned int : 6")]
        public uint num_mods
        {
            readonly get
            {
                return (_bitfield >> 24) & 0x3Fu;
            }

            set
            {
                _bitfield = (_bitfield & ~(0x3Fu << 24)) | ((value & 0x3Fu) << 24);
            }
        }

        [NativeTypeName("unsigned int : 1")]
        public uint byref
        {
            readonly get
            {
                return (_bitfield >> 30) & 0x1u;
            }

            set
            {
                _bitfield = (_bitfield & ~(0x1u << 30)) | ((value & 0x1u) << 30);
            }
        }

        [NativeTypeName("unsigned int : 1")]
        public uint pinned
        {
            readonly get
            {
                return (_bitfield >> 31) & 0x1u;
            }

            set
            {
                _bitfield = (_bitfield & ~(0x1u << 31)) | ((value & 0x1u) << 31);
            }
        }

        [NativeTypeName("MonoCustomMod[1]")]
        public _modifiers_e__FixedBuffer modifiers;

        [StructLayout(LayoutKind.Explicit)]
        public unsafe partial struct _data_e__Union
        {
            [FieldOffset(0)]
            [NativeTypeName("struct MonoClass *")]
            public MonoClass* klass;

            [FieldOffset(0)]
            public MonoType* type;

            [FieldOffset(0)]
            public MonoArrayType* array;

            [FieldOffset(0)]
            public MonoMethodSignature* method;

            [FieldOffset(0)]
            public MonoGenericParam* generic_param;

            [FieldOffset(0)]
            public MonoGenericClass* generic_class;
        }

        public partial struct _modifiers_e__FixedBuffer
        {
            public MonoCustomMod e0;

            [UnscopedRef]
            public ref MonoCustomMod this[int index]
            {
                get
                {
                    return ref Unsafe.Add(ref e0, index);
                }
            }

            [UnscopedRef]
            public Span<MonoCustomMod> AsSpan(int length) => MemoryMarshal.CreateSpan(ref e0, length);
        }
    }

    public unsafe partial struct MonoClassField
    {
        public MonoType* type;

        [NativeTypeName("const char *")]
        public sbyte* name;

        [NativeTypeName("struct MonoClass *")]
        public MonoClass* parent;

        public int offset;
    }

    public unsafe partial struct MonoVTable
    {
        [NativeTypeName("struct MonoClass *")]
        public MonoClass* klass;

        [NativeTypeName("MonoGCDescriptor")]
        public void* gc_descr;

        public void* domain;

        [NativeTypeName("gpointer")]
        public void* type;

        [NativeTypeName("guint8 *")]
        public byte* interface_bitmap;

        [NativeTypeName("guint16")]
        public ushort max_interface_id;

        [NativeTypeName("guint8")]
        public byte rank;

        [NativeTypeName("guint8")]
        public byte initialized;

        public uint _bitfield;

        [NativeTypeName("guint : 1")]
        public uint remote
        {
            readonly get
            {
                return _bitfield & 0x1u;
            }

            set
            {
                _bitfield = (_bitfield & ~0x1u) | (value & 0x1u);
            }
        }

        [NativeTypeName("guint : 1")]
        public uint init_failed
        {
            readonly get
            {
                return (_bitfield >> 1) & 0x1u;
            }

            set
            {
                _bitfield = (_bitfield & ~(0x1u << 1)) | ((value & 0x1u) << 1);
            }
        }

        [NativeTypeName("guint : 1")]
        public uint has_static_fields
        {
            readonly get
            {
                return (_bitfield >> 2) & 0x1u;
            }

            set
            {
                _bitfield = (_bitfield & ~(0x1u << 2)) | ((value & 0x1u) << 2);
            }
        }

        [NativeTypeName("guint : 4")]
        public uint gc_bits
        {
            readonly get
            {
                return (_bitfield >> 3) & 0xFu;
            }

            set
            {
                _bitfield = (_bitfield & ~(0xFu << 3)) | ((value & 0xFu) << 3);
            }
        }

        [NativeTypeName("guint32")]
        public uint imt_collisions_bitmap;

        [NativeTypeName("MonoRuntimeGenericContext *")]
        public void** runtime_generic_context;

        [NativeTypeName("gpointer[1]")]
        public _vtable_e__FixedBuffer vtable;

        public unsafe partial struct _vtable_e__FixedBuffer
        {
            public void* e0;

            public ref void* this[int index]
            {
                get
                {
                    fixed (void** pThis = &e0)
                    {
                        return ref pThis[index];
                    }
                }
            }
        }
    }

    public partial struct MonoClassRuntimeInfo
    {
        [NativeTypeName("guint16")]
        public ushort max_domain;

        [NativeTypeName("MonoVTable *[1]")]
        public _domain_vtables_e__FixedBuffer domain_vtables;

        public unsafe partial struct _domain_vtables_e__FixedBuffer
        {
            public MonoVTable* e0;

            public ref MonoVTable* this[int index]
            {
                get
                {
                    fixed (MonoVTable** pThis = &e0)
                    {
                        return ref pThis[index];
                    }
                }
            }
        }
    }

    public unsafe partial struct MonoClass
    {
        public MonoClass* element_class;

        public MonoClass* cast_class;

        public MonoClass** supertypes;

        [NativeTypeName("guint16")]
        public ushort idepth;

        [NativeTypeName("guint8")]
        public byte rank;

        public int instance_size;

        public uint _bitfield1;

        [NativeTypeName("guint : 1")]
        public uint inited
        {
            readonly get
            {
                return _bitfield1 & 0x1u;
            }

            set
            {
                _bitfield1 = (_bitfield1 & ~0x1u) | (value & 0x1u);
            }
        }

        [NativeTypeName("guint : 1")]
        public uint init_pending
        {
            readonly get
            {
                return (_bitfield1 >> 1) & 0x1u;
            }

            set
            {
                _bitfield1 = (_bitfield1 & ~(0x1u << 1)) | ((value & 0x1u) << 1);
            }
        }

        [NativeTypeName("guint : 1")]
        public uint size_inited
        {
            readonly get
            {
                return (_bitfield1 >> 2) & 0x1u;
            }

            set
            {
                _bitfield1 = (_bitfield1 & ~(0x1u << 2)) | ((value & 0x1u) << 2);
            }
        }

        [NativeTypeName("guint : 1")]
        public uint valuetype
        {
            readonly get
            {
                return (_bitfield1 >> 3) & 0x1u;
            }

            set
            {
                _bitfield1 = (_bitfield1 & ~(0x1u << 3)) | ((value & 0x1u) << 3);
            }
        }

        [NativeTypeName("guint : 1")]
        public uint enumtype
        {
            readonly get
            {
                return (_bitfield1 >> 4) & 0x1u;
            }

            set
            {
                _bitfield1 = (_bitfield1 & ~(0x1u << 4)) | ((value & 0x1u) << 4);
            }
        }

        [NativeTypeName("guint : 1")]
        public uint blittable
        {
            readonly get
            {
                return (_bitfield1 >> 5) & 0x1u;
            }

            set
            {
                _bitfield1 = (_bitfield1 & ~(0x1u << 5)) | ((value & 0x1u) << 5);
            }
        }

        [NativeTypeName("guint : 1")]
        public uint unicode
        {
            readonly get
            {
                return (_bitfield1 >> 6) & 0x1u;
            }

            set
            {
                _bitfield1 = (_bitfield1 & ~(0x1u << 6)) | ((value & 0x1u) << 6);
            }
        }

        [NativeTypeName("guint : 1")]
        public uint wastypebuilder
        {
            readonly get
            {
                return (_bitfield1 >> 7) & 0x1u;
            }

            set
            {
                _bitfield1 = (_bitfield1 & ~(0x1u << 7)) | ((value & 0x1u) << 7);
            }
        }

        [NativeTypeName("guint8")]
        public byte min_align;

        public uint _bitfield2;

        [NativeTypeName("guint : 4")]
        public uint packing_size
        {
            readonly get
            {
                return _bitfield2 & 0xFu;
            }

            set
            {
                _bitfield2 = (_bitfield2 & ~0xFu) | (value & 0xFu);
            }
        }

        [NativeTypeName("guint : 1")]
        public uint ghcimpl
        {
            readonly get
            {
                return (_bitfield2 >> 4) & 0x1u;
            }

            set
            {
                _bitfield2 = (_bitfield2 & ~(0x1u << 4)) | ((value & 0x1u) << 4);
            }
        }

        [NativeTypeName("guint : 1")]
        public uint has_finalize
        {
            readonly get
            {
                return (_bitfield2 >> 5) & 0x1u;
            }

            set
            {
                _bitfield2 = (_bitfield2 & ~(0x1u << 5)) | ((value & 0x1u) << 5);
            }
        }

        [NativeTypeName("guint : 1")]
        public uint marshalbyref
        {
            readonly get
            {
                return (_bitfield2 >> 6) & 0x1u;
            }

            set
            {
                _bitfield2 = (_bitfield2 & ~(0x1u << 6)) | ((value & 0x1u) << 6);
            }
        }

        [NativeTypeName("guint : 1")]
        public uint contextbound
        {
            readonly get
            {
                return (_bitfield2 >> 7) & 0x1u;
            }

            set
            {
                _bitfield2 = (_bitfield2 & ~(0x1u << 7)) | ((value & 0x1u) << 7);
            }
        }

        [NativeTypeName("guint : 1")]
        public uint @delegate
        {
            readonly get
            {
                return (_bitfield2 >> 8) & 0x1u;
            }

            set
            {
                _bitfield2 = (_bitfield2 & ~(0x1u << 8)) | ((value & 0x1u) << 8);
            }
        }

        [NativeTypeName("guint : 1")]
        public uint gc_descr_inited
        {
            readonly get
            {
                return (_bitfield2 >> 9) & 0x1u;
            }

            set
            {
                _bitfield2 = (_bitfield2 & ~(0x1u << 9)) | ((value & 0x1u) << 9);
            }
        }

        [NativeTypeName("guint : 1")]
        public uint has_cctor
        {
            readonly get
            {
                return (_bitfield2 >> 10) & 0x1u;
            }

            set
            {
                _bitfield2 = (_bitfield2 & ~(0x1u << 10)) | ((value & 0x1u) << 10);
            }
        }

        [NativeTypeName("guint : 1")]
        public uint has_references
        {
            readonly get
            {
                return (_bitfield2 >> 11) & 0x1u;
            }

            set
            {
                _bitfield2 = (_bitfield2 & ~(0x1u << 11)) | ((value & 0x1u) << 11);
            }
        }

        [NativeTypeName("guint : 1")]
        public uint has_static_refs
        {
            readonly get
            {
                return (_bitfield2 >> 12) & 0x1u;
            }

            set
            {
                _bitfield2 = (_bitfield2 & ~(0x1u << 12)) | ((value & 0x1u) << 12);
            }
        }

        [NativeTypeName("guint : 1")]
        public uint no_special_static_fields
        {
            readonly get
            {
                return (_bitfield2 >> 13) & 0x1u;
            }

            set
            {
                _bitfield2 = (_bitfield2 & ~(0x1u << 13)) | ((value & 0x1u) << 13);
            }
        }

        [NativeTypeName("guint : 1")]
        public uint is_com_object
        {
            readonly get
            {
                return (_bitfield2 >> 14) & 0x1u;
            }

            set
            {
                _bitfield2 = (_bitfield2 & ~(0x1u << 14)) | ((value & 0x1u) << 14);
            }
        }

        [NativeTypeName("guint : 1")]
        public uint nested_classes_inited
        {
            readonly get
            {
                return (_bitfield2 >> 15) & 0x1u;
            }

            set
            {
                _bitfield2 = (_bitfield2 & ~(0x1u << 15)) | ((value & 0x1u) << 15);
            }
        }

        [NativeTypeName("guint : 1")]
        public uint interfaces_inited
        {
            readonly get
            {
                return (_bitfield2 >> 16) & 0x1u;
            }

            set
            {
                _bitfield2 = (_bitfield2 & ~(0x1u << 16)) | ((value & 0x1u) << 16);
            }
        }

        [NativeTypeName("guint : 1")]
        public uint simd_type
        {
            readonly get
            {
                return (_bitfield2 >> 17) & 0x1u;
            }

            set
            {
                _bitfield2 = (_bitfield2 & ~(0x1u << 17)) | ((value & 0x1u) << 17);
            }
        }

        [NativeTypeName("guint : 1")]
        public uint is_generic
        {
            readonly get
            {
                return (_bitfield2 >> 18) & 0x1u;
            }

            set
            {
                _bitfield2 = (_bitfield2 & ~(0x1u << 18)) | ((value & 0x1u) << 18);
            }
        }

        [NativeTypeName("guint : 1")]
        public uint is_inflated
        {
            readonly get
            {
                return (_bitfield2 >> 19) & 0x1u;
            }

            set
            {
                _bitfield2 = (_bitfield2 & ~(0x1u << 19)) | ((value & 0x1u) << 19);
            }
        }

        [NativeTypeName("guint : 1")]
        public uint has_finalize_inited
        {
            readonly get
            {
                return (_bitfield2 >> 20) & 0x1u;
            }

            set
            {
                _bitfield2 = (_bitfield2 & ~(0x1u << 20)) | ((value & 0x1u) << 20);
            }
        }

        [NativeTypeName("guint : 1")]
        public uint fields_inited
        {
            readonly get
            {
                return (_bitfield2 >> 21) & 0x1u;
            }

            set
            {
                _bitfield2 = (_bitfield2 & ~(0x1u << 21)) | ((value & 0x1u) << 21);
            }
        }

        [NativeTypeName("guint : 1")]
        public uint setup_fields_called
        {
            readonly get
            {
                return (_bitfield2 >> 22) & 0x1u;
            }

            set
            {
                _bitfield2 = (_bitfield2 & ~(0x1u << 22)) | ((value & 0x1u) << 22);
            }
        }

        [NativeTypeName("guint8")]
        public byte exception_type;

        public MonoClass* parent;

        public MonoClass* nested_in;

        public void* image;

        [NativeTypeName("const char *")]
        public sbyte* name;

        [NativeTypeName("const char *")]
        public sbyte* name_space;

        [NativeTypeName("guint32")]
        public uint type_token;

        public int vtable_size;

        [NativeTypeName("guint16")]
        public ushort interface_count;

        [NativeTypeName("guint16")]
        public ushort interface_id;

        [NativeTypeName("guint16")]
        public ushort max_interface_id;

        [NativeTypeName("guint16")]
        public ushort interface_offsets_count;

        public MonoClass** interfaces_packed;

        [NativeTypeName("guint16 *")]
        public ushort* interface_offsets_packed;

        [NativeTypeName("guint8 *")]
        public byte* interface_bitmap;

        public MonoClass** interfaces;

        [NativeTypeName("__AnonymousRecord_input_L420_C3")]
        public _sizes_e__Union sizes;

        [NativeTypeName("guint32")]
        public uint flags;

        [NativeTypeName("__AnonymousRecord_input_L431_C3")]
        public _field_e__Struct field;

        [NativeTypeName("__AnonymousRecord_input_L431_C3")]
        public _field_e__Struct method;

        [NativeTypeName("guint32")]
        public uint ref_info_handle;

        public void* marshal_info;

        public MonoClassField* fields;

        public MonoMethod** methods;

        public MonoType this_arg;

        public MonoType byval_arg;

        public MonoGenericClass* generic_class;

        public MonoGenericContainer* generic_container;

        [NativeTypeName("MonoGCDescriptor")]
        public void* gc_descr;

        public MonoClassRuntimeInfo* runtime_info;

        public MonoClass* next_class_cache;

        public MonoMethod** vtable;

        public void* ext;

        public void* unity_user_data;

        [StructLayout(LayoutKind.Explicit)]
        public partial struct _sizes_e__Union
        {
            [FieldOffset(0)]
            public int class_size;

            [FieldOffset(0)]
            public int element_size;

            [FieldOffset(0)]
            public int generic_param_token;
        }

        public partial struct _field_e__Struct
        {
            [NativeTypeName("guint32")]
            public uint first;

            [NativeTypeName("guint32")]
            public uint count;
        }
    }

    public unsafe partial struct MonoProperty
    {
        public MonoClass* parent;

        [NativeTypeName("const char *")]
        public sbyte* name;

        public MonoMethod* get;

        public MonoMethod* set;

        [NativeTypeName("guint32")]
        public uint attrs;
    }

    public unsafe partial struct MonoEvent
    {
        public MonoClass* parent;

        [NativeTypeName("const char *")]
        public sbyte* name;

        public MonoMethod* add;

        public MonoMethod* remove;

        public MonoMethod* raise;

        public MonoMethod** other;

        [NativeTypeName("guint32")]
        public uint attrs;
    }

    public unsafe partial struct MonoFieldDefaultValue
    {
        [NativeTypeName("const char *")]
        public sbyte* data;

        public MonoTypeEnum def_type;
    }

    public unsafe partial struct MonoClassExt
    {
        [NativeTypeName("__AnonymousRecord_input_L514_C3")]
        public _property_e__Struct property;

        [NativeTypeName("__AnonymousRecord_input_L514_C3")]
        public _property_e__Struct @event;

        public MonoProperty* properties;

        public MonoEvent* events;

        [NativeTypeName("guint32")]
        public uint declsec_flags;

        public MonoFieldDefaultValue* field_def_values;

        public MonoFieldDefaultValue* prop_def_values;

        public void* nested_classes;

        public partial struct _property_e__Struct
        {
            [NativeTypeName("guint32")]
            public uint first;

            [NativeTypeName("guint32")]
            public uint count;
        }
    }
}
