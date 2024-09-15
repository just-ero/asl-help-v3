#include <cstdint>

typedef int8_t gint8;
typedef uint8_t guint8;
typedef int16_t gint16;
typedef uint16_t guint16;
typedef int32_t gint32;
typedef uint32_t guint32;
typedef int64_t gint64;
typedef uint64_t guint64;
typedef float gfloat;
typedef double gdouble;
typedef int32_t gboolean;

typedef int gint;
typedef unsigned int guint;
typedef short gshort;
typedef unsigned short gushort;
typedef long glong;
typedef unsigned long gulong;
typedef void *gpointer;
typedef const void *gconstpointer;
typedef char gchar;
typedef unsigned char guchar;

typedef int32_t mono_bool;
typedef uint8_t mono_byte;
typedef uint16_t mono_unichar2;
typedef uint32_t mono_unichar4;

struct MonoMemPool {
  // Next block after this one in linked list
  MonoMemPool *next;

  // Size of this memory block only
  guint32 size;

  // Used in "initial block" only: Beginning of current free space in mempool
  // (may be in some block other than the first one)
  guint8 *pos;

  // Used in "initial block" only: End of current free space in mempool (ie, the
  // first byte following the end of usable space)
  guint8 *end;

  union {
    // Unused: Imposing floating point memory rules on _MonoMemPool's final
    // field ensures proper alignment of whole header struct
    double pad;

    // Used in "initial block" only: Number of bytes so far allocated (whether
    // used or not) in the whole mempool
    guint32 allocated;
  } d;
};

struct MonoTableInfo {
  const char *base;
  guint rows : 24;
  guint row_size : 8;

  /*
   * Tables contain up to 9 columns and the possible sizes of the
   * fields in the documentation are 1, 2 and 4 bytes.  So we
   * can encode in 2 bits the size.
   *
   * A 32 bit value can encode the resulting size
   *
   * The top eight bits encode the number of columns in the table.
   * we only need 4, but 8 is aligned no shift required.
   */
  guint32 size_bitfield;
};

typedef struct {
  const char *data;
  guint32 size;
} MonoStreamHeader;

typedef enum {
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
  MONO_TABLE_MEMBERREF, /* 0xa */
  MONO_TABLE_CONSTANT,
  MONO_TABLE_CUSTOMATTRIBUTE,
  MONO_TABLE_FIELDMARSHAL,
  MONO_TABLE_DECLSECURITY,
  MONO_TABLE_CLASSLAYOUT,
  MONO_TABLE_FIELDLAYOUT, /* 0x10 */
  MONO_TABLE_STANDALONESIG,
  MONO_TABLE_EVENTMAP,
  MONO_TABLE_EVENT_POINTER,
  MONO_TABLE_EVENT,
  MONO_TABLE_PROPERTYMAP,
  MONO_TABLE_PROPERTY_POINTER,
  MONO_TABLE_PROPERTY,
  MONO_TABLE_METHODSEMANTICS,
  MONO_TABLE_METHODIMPL,
  MONO_TABLE_MODULEREF, /* 0x1a */
  MONO_TABLE_TYPESPEC,
  MONO_TABLE_IMPLMAP,
  MONO_TABLE_FIELDRVA,
  MONO_TABLE_UNUSED6,
  MONO_TABLE_UNUSED7,
  MONO_TABLE_ASSEMBLY, /* 0x20 */
  MONO_TABLE_ASSEMBLYPROCESSOR,
  MONO_TABLE_ASSEMBLYOS,
  MONO_TABLE_ASSEMBLYREF,
  MONO_TABLE_ASSEMBLYREFPROCESSOR,
  MONO_TABLE_ASSEMBLYREFOS,
  MONO_TABLE_FILE,
  MONO_TABLE_EXPORTEDTYPE,
  MONO_TABLE_MANIFESTRESOURCE,
  MONO_TABLE_NESTEDCLASS,
  MONO_TABLE_GENERICPARAM, /* 0x2a */
  MONO_TABLE_METHODSPEC,
  MONO_TABLE_GENERICPARAMCONSTRAINT,
  MONO_TABLE_UNUSED8,
  MONO_TABLE_UNUSED9,
  MONO_TABLE_UNUSED10,
  /* Portable PDB tables */
  MONO_TABLE_DOCUMENT, /* 0x30 */
  MONO_TABLE_METHODBODY,
  MONO_TABLE_LOCALSCOPE,
  MONO_TABLE_LOCALVARIABLE,
  MONO_TABLE_LOCALCONSTANT,
  MONO_TABLE_IMPORTSCOPE,
  MONO_TABLE_ASYNCMETHOD,
  MONO_TABLE_CUSTOMDEBUGINFORMATION

#define MONO_TABLE_LAST MONO_TABLE_CUSTOMDEBUGINFORMATION
#define MONO_TABLE_NUM (MONO_TABLE_LAST + 1)

} MonoMetaTableEnum;

#define MONO_PUBLIC_KEY_TOKEN_LENGTH 17

struct MonoAssemblyName {
  const char *name;
  const char *culture;
  const char *hash_value;
  const mono_byte *public_key;
  // string of 16 hex chars + 1 NULL
  mono_byte public_key_token[MONO_PUBLIC_KEY_TOKEN_LENGTH];
  uint32_t hash_alg;
  uint32_t hash_len;
  uint32_t flags;
  uint16_t major, minor, build, revision, arch;
};

struct GSList {
  gpointer data;
  GSList *next;
};

struct MonoAssembly {
  /*
   * The number of appdomains which have this assembly loaded plus the number of
   * assemblies referencing this assembly through an entry in their
   * image->references arrays. The latter is needed because entries in the
   * image->references array might point to assemblies which are only loaded in
   * some appdomains, and without the additional reference, they can be freed at
   * any time. The ref_count is initially 0.
   */
  int ref_count; /* use atomic operations only */
  char *basedir;
  MonoAssemblyName aname;
  struct MonoImage *image;
  GSList *friend_assembly_names; /* Computed by mono_assembly_load_friends () */
  guint8 friend_assembly_names_inited;
  guint8 in_gac;
  guint8 dynamic;
  guint8 corlib_internal;
  gboolean ref_only;
  guint8 wrap_non_exception_throws;
  guint8 wrap_non_exception_throws_inited;
  guint8 jit_optimizer_disabled;
  guint8 jit_optimizer_disabled_inited;
  /* security manager flags (one bit is for lazy initialization) */
  guint32 ecma : 2;      /* Has the ECMA key */
  guint32 aptc : 2;      /* Has the [AllowPartiallyTrustedCallers] attributes */
  guint32 fulltrust : 2; /* Has FullTrust permission */
  guint32
      unmanaged : 2; /* Has SecurityPermissionFlag.UnmanagedCode permission */
  guint32 skipverification : 2; /* Has SecurityPermissionFlag.SkipVerification
                                   permission */
};

typedef void (*GDestroyNotify)(gpointer data);
typedef guint (*GHashFunc)(gconstpointer key);
typedef gboolean (*GEqualFunc)(gconstpointer a, gconstpointer b);

struct Slot {
  gpointer key;
  gpointer value;
  Slot *next;
};

struct GHashTable {
  GHashFunc hash_func;
  GEqualFunc key_equal_func;

  Slot **table;
  int table_size;
  int in_use;
  int threshold;
  int last_rehash;
  GDestroyNotify value_destroy_func, key_destroy_func;
};

typedef gpointer (*MonoInternalHashKeyExtractFunc)(gpointer value);
typedef gpointer *(*MonoInternalHashNextValueFunc)(gpointer value);

struct MonoInternalHashTable {
  GHashFunc hash_func;
  MonoInternalHashKeyExtractFunc key_extract;
  MonoInternalHashNextValueFunc next_value;
  gint size;
  gint num_entries;
  gpointer *table;
};

struct MonoImage {
  /*
   * The number of assemblies which reference this MonoImage though their
   * 'image' field plus the number of images which reference this MonoImage
   * through their 'modules' field, plus the number of threads holding temporary
   * references to this image between calls of mono_image_open () and
   * mono_image_close ().
   */
  int ref_count;

  /* If the raw data was allocated from a source such as mmap, the allocator may
   * store resource tracking information here. */
  void *raw_data_handle;
  char *raw_data;
  guint32 raw_data_len;
  guint8 raw_buffer_used : 1;
  guint8 raw_data_allocated : 1;
  guint8 fileio_used : 1;

#ifdef HOST_WIN32
  /* Module was loaded using LoadLibrary. */
  guint8 is_module_handle : 1;

  /* Module entry point is _CorDllMain. */
  guint8 has_entry_point : 1;
#endif

  /* Whenever this is a dynamically emitted module */
  guint8 dynamic : 1;

  /* Whenever this is a reflection only image */
  guint8 ref_only : 1;

  /* Whenever this image contains uncompressed metadata */
  guint8 uncompressed_metadata : 1;

  /* Whenever this image contains metadata only without PE data */
  guint8 metadata_only : 1;

  guint8 checked_module_cctor : 1;
  guint8 has_module_cctor : 1;

  guint8 idx_string_wide : 1;
  guint8 idx_guid_wide : 1;
  guint8 idx_blob_wide : 1;

  /* Whenever this image is considered as platform code for the CoreCLR security
   * model */
  guint8 core_clr_platform_code : 1;

  /* The path to the file for this image. */
  char *name;

  /* The assembly name reported in the file for this image (expected to be NULL
   * for a netmodule) */
  const char *assembly_name;

  /* The module name reported in the file for this image (could be NULL for a
   * malformed file) */
  const char *module_name;

  char *version;
  gint16 md_version_major, md_version_minor;
  char *guid;
  void *image_info;
  MonoMemPool *mempool; /*protected by the image lock*/

  char *raw_metadata;

  MonoStreamHeader heap_strings;
  MonoStreamHeader heap_us;
  MonoStreamHeader heap_blob;
  MonoStreamHeader heap_guid;
  MonoStreamHeader heap_tables;
  MonoStreamHeader heap_pdb;

  const char *tables_base;

  /**/
  MonoTableInfo tables[MONO_TABLE_NUM];

  /*
   * references is initialized only by using the mono_assembly_open
   * function, and not by using the lowlevel mono_image_open.
   *
   * It is NULL terminated.
   */
  MonoAssembly **references;
  int nreferences;

  /* Code files in the assembly. */
  MonoImage **modules;
  guint32 module_count;
  gboolean *modules_loaded;

  /*
   * Files in the assembly. Items are either NULL or alias items in modules, so
   * this does not impact ref_count. Protected by the image lock.
   */
  MonoImage **files;

  gpointer aot_module;

  guint8 aotid[16];

  /*
   * The Assembly this image was loaded from.
   */
  MonoAssembly *assembly;

  /*
   * Indexed by method tokens and typedef tokens.
   */
  GHashTable *method_cache; /*protected by the image lock*/
  MonoInternalHashTable class_cache;
};
