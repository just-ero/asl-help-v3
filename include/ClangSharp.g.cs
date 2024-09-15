using System;
using System.Runtime.InteropServices;

namespace ClangSharp.Generated
{
    internal unsafe struct MonoMemPool
    {
        public MonoMemPool* next;

        public uint size;

        public byte* pos;

        public byte* end;

        public _d_e__Union d;

        [StructLayout(LayoutKind.Explicit)]
        public struct _d_e__Union
        {
            [FieldOffset(0)]
            public double pad;

            [FieldOffset(0)]
            public uint allocated;
        }
    }

    internal unsafe struct MonoTableInfo
    {
        public sbyte* @base;

        public uint _bitfield;

        public uint rows
        {
            readonly get
            {
                return _bitfield & 0xFFFFFFu;
            }

            set
            {
                _bitfield = (_bitfield & ~0xFFFFFFu) | (value & 0xFFFFFFu);
            }
        }

        public uint row_size
        {
            readonly get
            {
                return (_bitfield >> 24) & 0xFFu;
            }

            set
            {
                _bitfield = (_bitfield & ~(0xFFu << 24)) | ((value & 0xFFu) << 24);
            }
        }

        public uint size_bitfield;
    }

    internal unsafe struct MonoStreamHeader
    {
        public sbyte* data;

        public uint size;
    }

    public enum MonoMetaTableEnum
    {
        MONO_TABLE_MODULE,
        MONO_TABLE_TYPEREF,
        MONO_TABLE_TYPEDEF,
        MONO_TABLE_FIELD_POINTER,
        MONO_TABLE_FIELD,
        MONO_TABLE_METHOD_POINTER,
        MONO_TABLE_METHOD,
        MONO_TABLE_PARAM_POINTER,
        MONO_TABLE_PARAM,
        MONO_TABLE_INTERFACEIMPL,
        MONO_TABLE_MEMBERREF,
        MONO_TABLE_CONSTANT,
        MONO_TABLE_CUSTOMATTRIBUTE,
        MONO_TABLE_FIELDMARSHAL,
        MONO_TABLE_DECLSECURITY,
        MONO_TABLE_CLASSLAYOUT,
        MONO_TABLE_FIELDLAYOUT,
        MONO_TABLE_STANDALONESIG,
        MONO_TABLE_EVENTMAP,
        MONO_TABLE_EVENT_POINTER,
        MONO_TABLE_EVENT,
        MONO_TABLE_PROPERTYMAP,
        MONO_TABLE_PROPERTY_POINTER,
        MONO_TABLE_PROPERTY,
        MONO_TABLE_METHODSEMANTICS,
        MONO_TABLE_METHODIMPL,
        MONO_TABLE_MODULEREF,
        MONO_TABLE_TYPESPEC,
        MONO_TABLE_IMPLMAP,
        MONO_TABLE_FIELDRVA,
        MONO_TABLE_UNUSED6,
        MONO_TABLE_UNUSED7,
        MONO_TABLE_ASSEMBLY,
        MONO_TABLE_ASSEMBLYPROCESSOR,
        MONO_TABLE_ASSEMBLYOS,
        MONO_TABLE_ASSEMBLYREF,
        MONO_TABLE_ASSEMBLYREFPROCESSOR,
        MONO_TABLE_ASSEMBLYREFOS,
        MONO_TABLE_FILE,
        MONO_TABLE_EXPORTEDTYPE,
        MONO_TABLE_MANIFESTRESOURCE,
        MONO_TABLE_NESTEDCLASS,
        MONO_TABLE_GENERICPARAM,
        MONO_TABLE_METHODSPEC,
        MONO_TABLE_GENERICPARAMCONSTRAINT,
        MONO_TABLE_UNUSED8,
        MONO_TABLE_UNUSED9,
        MONO_TABLE_UNUSED10,
        MONO_TABLE_DOCUMENT,
        MONO_TABLE_METHODBODY,
        MONO_TABLE_LOCALSCOPE,
        MONO_TABLE_LOCALVARIABLE,
        MONO_TABLE_LOCALCONSTANT,
        MONO_TABLE_IMPORTSCOPE,
        MONO_TABLE_ASYNCMETHOD,
        MONO_TABLE_CUSTOMDEBUGINFORMATION,
    }

    internal unsafe struct MonoAssemblyName
    {
        public sbyte* name;

        public sbyte* culture;

        public sbyte* hash_value;

        public byte* public_key;

        public fixed byte public_key_token[17];

        public uint hash_alg;

        public uint hash_len;

        public uint flags;

        public ushort major;

        public ushort minor;

        public ushort build;

        public ushort revision;

        public ushort arch;
    }

    internal unsafe struct GSList
    {
        public void* data;

        public GSList* next;
    }

    internal unsafe struct MonoAssembly
    {
        public int ref_count;

        public sbyte* basedir;

        public MonoAssemblyName aname;

        public MonoImage* image;

        public GSList* friend_assembly_names;

        public byte friend_assembly_names_inited;

        public byte in_gac;

        public byte dynamic;

        public byte corlib_internal;

        public int ref_only;

        public byte wrap_non_exception_throws;

        public byte wrap_non_exception_throws_inited;

        public byte jit_optimizer_disabled;

        public byte jit_optimizer_disabled_inited;

        public uint _bitfield;

        public uint ecma
        {
            readonly get
            {
                return _bitfield & 0x3u;
            }

            set
            {
                _bitfield = (_bitfield & ~0x3u) | (value & 0x3u);
            }
        }

        public uint aptc
        {
            readonly get
            {
                return (_bitfield >> 2) & 0x3u;
            }

            set
            {
                _bitfield = (_bitfield & ~(0x3u << 2)) | ((value & 0x3u) << 2);
            }
        }

        public uint fulltrust
        {
            readonly get
            {
                return (_bitfield >> 4) & 0x3u;
            }

            set
            {
                _bitfield = (_bitfield & ~(0x3u << 4)) | ((value & 0x3u) << 4);
            }
        }

        public uint unmanaged
        {
            readonly get
            {
                return (_bitfield >> 6) & 0x3u;
            }

            set
            {
                _bitfield = (_bitfield & ~(0x3u << 6)) | ((value & 0x3u) << 6);
            }
        }

        public uint skipverification
        {
            readonly get
            {
                return (_bitfield >> 8) & 0x3u;
            }

            set
            {
                _bitfield = (_bitfield & ~(0x3u << 8)) | ((value & 0x3u) << 8);
            }
        }
    }

    internal unsafe struct Slot
    {
        public void* key;

        public void* value;

        public Slot* next;
    }

    internal unsafe struct GHashTable
    {
        public delegate* unmanaged[Cdecl]<void*, uint> hash_func;

        public delegate* unmanaged[Cdecl]<void*, void*, int> key_equal_func;

        public Slot** table;

        public int table_size;

        public int in_use;

        public int threshold;

        public int last_rehash;

        public delegate* unmanaged[Cdecl]<void*, void> value_destroy_func;

        public delegate* unmanaged[Cdecl]<void*, void> key_destroy_func;
    }

    internal unsafe struct MonoInternalHashTable
    {
        public delegate* unmanaged[Cdecl]<void*, uint> hash_func;

        public delegate* unmanaged[Cdecl]<void*, void*> key_extract;

        public delegate* unmanaged[Cdecl]<void*, void**> next_value;

        public int size;

        public int num_entries;

        public void** table;
    }

    internal unsafe struct MonoImage
    {
        public int ref_count;

        public void* raw_data_handle;

        public sbyte* raw_data;

        public uint raw_data_len;

        public byte _bitfield1;

        public byte raw_buffer_used
        {
            readonly get
            {
                return (byte)(_bitfield1 & 0x1u);
            }

            set
            {
                _bitfield1 = (byte)((_bitfield1 & ~0x1u) | (value & 0x1u));
            }
        }

        public byte raw_data_allocated
        {
            readonly get
            {
                return (byte)((_bitfield1 >> 1) & 0x1u);
            }

            set
            {
                _bitfield1 = (byte)((_bitfield1 & ~(0x1u << 1)) | ((value & 0x1u) << 1));
            }
        }

        public byte fileio_used
        {
            readonly get
            {
                return (byte)((_bitfield1 >> 2) & 0x1u);
            }

            set
            {
                _bitfield1 = (byte)((_bitfield1 & ~(0x1u << 2)) | ((value & 0x1u) << 2));
            }
        }

        public byte dynamic
        {
            readonly get
            {
                return (byte)((_bitfield1 >> 3) & 0x1u);
            }

            set
            {
                _bitfield1 = (byte)((_bitfield1 & ~(0x1u << 3)) | ((value & 0x1u) << 3));
            }
        }

        public byte ref_only
        {
            readonly get
            {
                return (byte)((_bitfield1 >> 4) & 0x1u);
            }

            set
            {
                _bitfield1 = (byte)((_bitfield1 & ~(0x1u << 4)) | ((value & 0x1u) << 4));
            }
        }

        public byte uncompressed_metadata
        {
            readonly get
            {
                return (byte)((_bitfield1 >> 5) & 0x1u);
            }

            set
            {
                _bitfield1 = (byte)((_bitfield1 & ~(0x1u << 5)) | ((value & 0x1u) << 5));
            }
        }

        public byte metadata_only
        {
            readonly get
            {
                return (byte)((_bitfield1 >> 6) & 0x1u);
            }

            set
            {
                _bitfield1 = (byte)((_bitfield1 & ~(0x1u << 6)) | ((value & 0x1u) << 6));
            }
        }

        public byte checked_module_cctor
        {
            readonly get
            {
                return (byte)((_bitfield1 >> 7) & 0x1u);
            }

            set
            {
                _bitfield1 = (byte)((_bitfield1 & ~(0x1u << 7)) | ((value & 0x1u) << 7));
            }
        }

        public byte _bitfield2;

        public byte has_module_cctor
        {
            readonly get
            {
                return (byte)(_bitfield2 & 0x1u);
            }

            set
            {
                _bitfield2 = (byte)((_bitfield2 & ~0x1u) | (value & 0x1u));
            }
        }

        public byte idx_string_wide
        {
            readonly get
            {
                return (byte)((_bitfield2 >> 1) & 0x1u);
            }

            set
            {
                _bitfield2 = (byte)((_bitfield2 & ~(0x1u << 1)) | ((value & 0x1u) << 1));
            }
        }

        public byte idx_guid_wide
        {
            readonly get
            {
                return (byte)((_bitfield2 >> 2) & 0x1u);
            }

            set
            {
                _bitfield2 = (byte)((_bitfield2 & ~(0x1u << 2)) | ((value & 0x1u) << 2));
            }
        }

        public byte idx_blob_wide
        {
            readonly get
            {
                return (byte)((_bitfield2 >> 3) & 0x1u);
            }

            set
            {
                _bitfield2 = (byte)((_bitfield2 & ~(0x1u << 3)) | ((value & 0x1u) << 3));
            }
        }

        public byte core_clr_platform_code
        {
            readonly get
            {
                return (byte)((_bitfield2 >> 4) & 0x1u);
            }

            set
            {
                _bitfield2 = (byte)((_bitfield2 & ~(0x1u << 4)) | ((value & 0x1u) << 4));
            }
        }

        public sbyte* name;

        public sbyte* assembly_name;

        public sbyte* module_name;

        public sbyte* version;

        public short md_version_major;

        public short md_version_minor;

        public sbyte* guid;

        public void* image_info;

        public MonoMemPool* mempool;

        public sbyte* raw_metadata;

        public MonoStreamHeader heap_strings;

        public MonoStreamHeader heap_us;

        public MonoStreamHeader heap_blob;

        public MonoStreamHeader heap_guid;

        public MonoStreamHeader heap_tables;

        public MonoStreamHeader heap_pdb;

        public sbyte* tables_base;

        public _tables_e__FixedBuffer tables;

        public MonoAssembly** references;

        public int nreferences;

        public MonoImage** modules;

        public uint module_count;

        public int* modules_loaded;

        public MonoImage** files;

        public void* aot_module;

        public fixed byte aotid[16];

        public MonoAssembly* assembly;

        public GHashTable* method_cache;

        public MonoInternalHashTable class_cache;

        public struct _tables_e__FixedBuffer
        {
            public MonoTableInfo e0;
            public MonoTableInfo e1;
            public MonoTableInfo e2;
            public MonoTableInfo e3;
            public MonoTableInfo e4;
            public MonoTableInfo e5;
            public MonoTableInfo e6;
            public MonoTableInfo e7;
            public MonoTableInfo e8;
            public MonoTableInfo e9;
            public MonoTableInfo e10;
            public MonoTableInfo e11;
            public MonoTableInfo e12;
            public MonoTableInfo e13;
            public MonoTableInfo e14;
            public MonoTableInfo e15;
            public MonoTableInfo e16;
            public MonoTableInfo e17;
            public MonoTableInfo e18;
            public MonoTableInfo e19;
            public MonoTableInfo e20;
            public MonoTableInfo e21;
            public MonoTableInfo e22;
            public MonoTableInfo e23;
            public MonoTableInfo e24;
            public MonoTableInfo e25;
            public MonoTableInfo e26;
            public MonoTableInfo e27;
            public MonoTableInfo e28;
            public MonoTableInfo e29;
            public MonoTableInfo e30;
            public MonoTableInfo e31;
            public MonoTableInfo e32;
            public MonoTableInfo e33;
            public MonoTableInfo e34;
            public MonoTableInfo e35;
            public MonoTableInfo e36;
            public MonoTableInfo e37;
            public MonoTableInfo e38;
            public MonoTableInfo e39;
            public MonoTableInfo e40;
            public MonoTableInfo e41;
            public MonoTableInfo e42;
            public MonoTableInfo e43;
            public MonoTableInfo e44;
            public MonoTableInfo e45;
            public MonoTableInfo e46;
            public MonoTableInfo e47;
            public MonoTableInfo e48;
            public MonoTableInfo e49;
            public MonoTableInfo e50;
            public MonoTableInfo e51;
            public MonoTableInfo e52;
            public MonoTableInfo e53;
            public MonoTableInfo e54;
            public MonoTableInfo e55;

            public ref MonoTableInfo this[int index]
            {
                get
                {
                    return ref AsSpan()[index];
                }
            }

            public Span<MonoTableInfo> AsSpan() => MemoryMarshal.CreateSpan(ref e0, 56);
        }
    }
}
