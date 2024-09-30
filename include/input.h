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

struct MonoMethod
{
  guint16 flags;  /* method flags */
  guint16 iflags; /* method implementation flags */
  guint32 token;
  struct MonoClass *klass; /* To what class does this method belong */
  struct MonoMethodSignature *signature;
  /* name is useful mostly for debugging */
  const char *name;
#ifdef IL2CPP_ON_MONO
  void *method_pointer;
  void *invoke_pointer;
#endif
  /* this is used by the inlining algorithm */
  unsigned int inline_info : 1;
  unsigned int inline_failure : 1;
  unsigned int wrapper_type : 5;
  unsigned int string_ctor : 1;
  unsigned int save_lmf : 1;
  unsigned int dynamic : 1;              /* created & destroyed during runtime */
  unsigned int sre_method : 1;           /* created at runtime using Reflection.Emit */
  unsigned int is_generic : 1;           /* whenever this is a generic method definition */
  unsigned int is_inflated : 1;          /* whether we're a MonoMethodInflated */
  unsigned int skip_visibility : 1;      /* whenever to skip JIT visibility checks */
  unsigned int verification_success : 1; /* whether this method has been verified successfully.*/
  signed int slot : 16;

  /*
   * If is_generic is TRUE, the generic_container is stored in image->property_hash,
   * using the key MONO_METHOD_PROP_GENERIC_CONTAINER.
   */
};

struct MonoGenericInst
{
#ifndef MONO_SMALL_CONFIG
  guint id; /* unique ID for debugging */
#endif
  guint type_argc : 22; /* number of type arguments */
  guint is_open : 1;    /* if this is an open type */
  struct MonoType *type_argv[1];
};

struct MonoGenericContext
{
  /* The instantiation corresponding to the class generic parameters */
  MonoGenericInst *class_inst;
  /* The instantiation corresponding to the method generic parameters */
  MonoGenericInst *method_inst;
};

struct MonoGenericContainer
{
  MonoGenericContext context;
  /* If we're a generic method definition in a generic type definition,
     the generic container of the containing class. */
  MonoGenericContainer *parent;
  /* the generic type definition or the generic method definition corresponding to this container */
  /* Union rules: If is_anonymous, image field is valid; else if is_method, method field is valid; else klass is valid. */
  union
  {
    struct MonoClass *klass;
    MonoMethod *method;
    void *image;
  } owner;
  int type_argc : 29; // Per the ECMA spec, this value is capped at 16 bits
  /* If true, we're a generic method, otherwise a generic type definition. */
  /* Invariant: parent != NULL => is_method */
  int is_method : 1;
  /* If true, this container has no associated class/method and only the image is known. This can happen:
     1. For the special anonymous containers kept by MonoImage.
     2. During container creation via the mono_metadata_load_generic_params path-- in this case the caller
        sets the owner, so temporarily while load_generic_params is completing the container is anonymous.
     3. When user code creates a generic parameter via SRE, but has not yet set an owner. */
  int is_anonymous : 1;
  /* If false, all params in this container are full-size. If true, all params are just param structs. */
  /* This field is always == to the is_anonymous field, except in "temporary" cases (2) and (3) above. */
  /* TODO: Merge GenericParam and GenericParamFull, remove this field. Benefit is marginal. */
  int is_small_param : 1;
  /* Our type parameters. */
  void *type_params;
};

struct MonoGenericParam
{
  /*
   * Type or method this parameter was defined in.
   */
  MonoGenericContainer *owner;
  guint16 num;
  /*
   * If != NULL, this is a generated generic param used by the JIT to implement generic
   * sharing.
   */
  struct MonoType *gshared_constraint;
};

typedef struct
{
  struct MonoClass *pklass; /* The corresponding `MonoClass'. */
  const char *name;

  // See GenericParameterAttributes
  guint16 flags;

  guint32 token;

  // Constraints on type parameters
  struct MonoClass **constraints; /* NULL means end of list */
} MonoGenericParamInfo;

typedef struct
{
  MonoGenericParam param;
  MonoGenericParamInfo info;
} MonoGenericParamFull;

struct MonoMethodSignature
{
  struct MonoType *ret;
#ifdef MONO_SMALL_CONFIG
  guint8 param_count;
  gint8 sentinelpos;
  unsigned int generic_param_count : 5;
#else
  guint16 param_count;
  gint16 sentinelpos;
  unsigned int generic_param_count : 16;
#endif
  unsigned int call_convention : 6;
  unsigned int hasthis : 1;
  unsigned int explicit_this : 1;
  unsigned int pinvoke : 1;
  unsigned int is_inflated : 1;
  unsigned int has_type_parameters : 1;
  struct MonoType *params[1];
};

struct MonoArrayType
{
  struct MonoClass *eklass;
  // Number of dimensions of the array
  uint8_t rank;

  // Arrays recording known upper and lower index bounds for each dimension
  uint8_t numsizes;
  uint8_t numlobounds;
  int *sizes;
  int *lobounds;
};

struct MonoGenericClass
{
  struct MonoClass *container_class;                                                                                        /* the generic type definition */
  MonoGenericContext context; /* a context that contains the type instantiation doesn't contain any method instantiation */ /* FIXME: Only the class_inst member of "context" is ever used, so this field could be replaced with just a monogenericinst */
  guint is_dynamic : 1;                                                                                                     /* Contains dynamic types */
  guint is_tb_open : 1;                                                                                                     /* This is the fully open instantiation for a type_builder. Quite ugly, but it's temporary.*/
  guint need_sync : 1;                                                                                                      /* Only if dynamic. Need to be synchronized with its container class after its finished. */
  struct MonoClass *cached_class;                                                                                           /* if present, the MonoClass corresponding to the instantiation.  */
};

typedef enum
{
  MONO_TYPE_END = 0x00, /* End of List */
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
  MONO_TYPE_PTR = 0x0f,         /* arg: <type> token */
  MONO_TYPE_BYREF = 0x10,       /* arg: <type> token */
  MONO_TYPE_VALUETYPE = 0x11,   /* arg: <type> token */
  MONO_TYPE_CLASS = 0x12,       /* arg: <type> token */
  MONO_TYPE_VAR = 0x13,         /* number */
  MONO_TYPE_ARRAY = 0x14,       /* type, rank, boundsCount, bound1, loCount, lo1 */
  MONO_TYPE_GENERICINST = 0x15, /* <type> <type-arg-count> <type-1> \x{2026} <type-n> */
  MONO_TYPE_TYPEDBYREF = 0x16,
  MONO_TYPE_I = 0x18,
  MONO_TYPE_U = 0x19,
  MONO_TYPE_FNPTR = 0x1b, /* arg: full method signature */
  MONO_TYPE_OBJECT = 0x1c,
  MONO_TYPE_SZARRAY = 0x1d,   /* 0-based one-dim-array */
  MONO_TYPE_MVAR = 0x1e,      /* number */
  MONO_TYPE_CMOD_REQD = 0x1f, /* arg: typedef or typeref token */
  MONO_TYPE_CMOD_OPT = 0x20,  /* optional arg: typedef or typref token */
  MONO_TYPE_INTERNAL = 0x21,  /* CLR internal type */

  MONO_TYPE_MODIFIER = 0x40, /* Or with the following types */
  MONO_TYPE_SENTINEL = 0x41, /* Sentinel for varargs method signature */
  MONO_TYPE_PINNED = 0x45,   /* Local var that points to pinned object */

  MONO_TYPE_ENUM = 0x55 /* an enumeration */
} MonoTypeEnum;

typedef struct
{
  unsigned int required : 1;
  unsigned int token : 31;
} MonoCustomMod;

struct MonoType
{
  union
  {
    struct MonoClass *klass; /* for VALUETYPE and CLASS */
    MonoType *type;          /* for PTR */
    MonoArrayType *array;    /* for ARRAY */
    MonoMethodSignature *method;
    MonoGenericParam *generic_param; /* for VAR and MVAR */
    MonoGenericClass *generic_class; /* for GENERICINST */
  } data;
  unsigned int attrs : 16; /* param attributes or field flags */
  MonoTypeEnum type : 8;
  unsigned int num_mods : 6; /* max 64 modifiers follow at the end */
  unsigned int byref : 1;
  unsigned int pinned : 1;    /* valid when included in a local var signature */
  MonoCustomMod modifiers[1]; /* this may grow */
};

struct MonoClassField
{
  /* Type of the field */
  MonoType *type;

  const char *name;

  /* Type where the field was defined */
  struct MonoClass *parent;

  /*
   * Offset where this field is stored; if it is an instance
   * field, it's the offset from the start of the object, if
   * it's static, it's from the start of the memory chunk
   * allocated for statics for the class.
   * For special static fields, this is set to -1 during vtable construction.
   */
  int offset;
};

typedef void *MonoGCDescriptor;
typedef gpointer MonoRuntimeGenericContext;

#define MONO_VTABLE_AVAILABLE_GC_BITS 4

struct MonoVTable
{
  struct MonoClass *klass;
  /*
   * According to comments in gc_gcj.h, this should be the second word in
   * the vtable.
   */
  MonoGCDescriptor gc_descr;
  void *domain;  /* each object/vtable belongs to exactly one domain */
  gpointer type; /* System.Type type for klass */
  guint8 *interface_bitmap;
  guint16 max_interface_id;
  guint8 rank;
  /* Keep this a guint8, the jit depends on it */
  guint8 initialized;                            /* cctor has been run */
  guint remote : 1;                              /* class is remotely activated */
  guint init_failed : 1;                         /* cctor execution failed */
  guint has_static_fields : 1;                   /* pointer to the data stored at the end of the vtable array */
  guint gc_bits : MONO_VTABLE_AVAILABLE_GC_BITS; /* Those bits are reserved for the usaged of the GC */

  guint32 imt_collisions_bitmap;
  MonoRuntimeGenericContext *runtime_generic_context;
  /* do not add any fields after vtable, the structure is dynamically extended */
  /* vtable contains function pointers to methods or their trampolines, at the
   end there may be a slot containing the pointer to the static fields */
  gpointer vtable[1];
};

typedef struct
{
  guint16 max_domain;
  /* domain_vtables is indexed by the domain id and the size is max_domain + 1 */
  MonoVTable *domain_vtables[1];
} MonoClassRuntimeInfo;

struct MonoClass
{
  /* element class for arrays and enum basetype for enums */
  MonoClass *element_class;
  /* used for subtype checks */
  MonoClass *cast_class;

  /* for fast subtype checks */
  MonoClass **supertypes;
  guint16 idepth;

  /* array dimension */
  guint8 rank;

  int instance_size; /* object instance size */

  guint inited : 1;
  /* We use init_pending to detect cyclic calls to mono_class_init */
  guint init_pending : 1;

  /* A class contains static and non static data. Static data can be
   * of the same type as the class itselfs, but it does not influence
   * the instance size of the class. To avoid cyclic calls to
   * mono_class_init (from mono_class_instance_size ()) we first
   * initialise all non static fields. After that we set size_inited
   * to 1, because we know the instance size now. After that we
   * initialise all static fields.
   */
  /* size_inited is accessed without locks, so it needs a memory barrier */
  guint size_inited : 1;
  guint valuetype : 1;      /* derives from System.ValueType */
  guint enumtype : 1;       /* derives from System.Enum */
  guint blittable : 1;      /* class is blittable */
  guint unicode : 1;        /* class uses unicode char when marshalled */
  guint wastypebuilder : 1; /* class was created at runtime from a TypeBuilder */
  /* next byte */
  guint8 min_align;

  /* next byte */
  guint packing_size : 4;
  guint ghcimpl : 1;      /* class has its own GetHashCode impl */
  guint has_finalize : 1; /* class has its own Finalize impl */
#ifndef DISABLE_REMOTING
  guint marshalbyref : 1; /* class is a MarshalByRefObject */
  guint contextbound : 1; /* class is a ContextBoundObject */
#endif
  /* next byte */
  guint delegate : 1;                 /* class is a Delegate */
  guint gc_descr_inited : 1;          /* gc_descr is initialized */
  guint has_cctor : 1;                /* class has a cctor */
  guint has_references : 1;           /* it has GC-tracked references in the instance */
  guint has_static_refs : 1;          /* it has static fields that are GC-tracked */
  guint no_special_static_fields : 1; /* has no thread/context static fields */
  /* directly or indirectly derives from ComImport attributed class.
   * this means we need to create a proxy for instances of this class
   * for COM Interop. set this flag on loading so all we need is a quick check
   * during object creation rather than having to traverse supertypes
   */
  guint is_com_object : 1;
  guint nested_classes_inited : 1; /* Whenever nested_class is initialized */

  /* next byte*/
  guint interfaces_inited : 1;   /* interfaces is initialized */
  guint simd_type : 1;           /* class is a simd intrinsic type */
  guint is_generic : 1;          /* class is a generic type definition */
  guint is_inflated : 1;         /* class is a generic instance */
  guint has_finalize_inited : 1; /* has_finalize is initialized */
  guint fields_inited : 1;       /* fields is initialized */
  guint setup_fields_called : 1; /* to prevent infinite loops in setup_fields */

  guint8 exception_type; /* MONO_EXCEPTION_* */

  /* Additional information about the exception */
  /* Stored as property MONO_CLASS_PROP_EXCEPTION_DATA */
  // void       *exception_data;

  MonoClass *parent;
  MonoClass *nested_in;

  void *image;
  const char *name;
  const char *name_space;

  guint32 type_token;
  int vtable_size; /* number of slots */

  guint16 interface_count;
  guint16 interface_id; /* unique inderface id (for interfaces) */
  guint16 max_interface_id;

  guint16 interface_offsets_count;
  MonoClass **interfaces_packed;
  guint16 *interface_offsets_packed;
/* enabled only with small config for now: we might want to do it unconditionally */
#ifdef MONO_SMALL_CONFIG
#define COMPRESSED_INTERFACE_BITMAP 1
#endif
  guint8 *interface_bitmap;

  MonoClass **interfaces;

  union
  {
    int class_size;          /* size of area for static fields */
    int element_size;        /* for array types */
    int generic_param_token; /* for generic param types, both var and mvar */
  } sizes;

  /*
   * From the TypeDef table
   */
  guint32 flags;
  struct
  {
#if MONO_SMALL_CONFIG
    guint16 first, count;
#else
    guint32 first, count;
#endif
  } field, method;

  /* A GC handle pointing to the corresponding type builder/generic param builder */
  guint32 ref_info_handle;

  /* loaded on demand */
  void *marshal_info;

  /*
   * Field information: Type and location from object base
   */
  MonoClassField *fields;

  MonoMethod **methods;

  /* used as the type of the this argument and when passing the arg by value */
  MonoType this_arg;
  MonoType byval_arg;

  MonoGenericClass *generic_class;
  MonoGenericContainer *generic_container;

  MonoGCDescriptor gc_descr;

  MonoClassRuntimeInfo *runtime_info;

  /* next element in the class_cache hash list (in MonoImage) */
  MonoClass *next_class_cache;

  /* Generic vtable. Initialized by a call to mono_class_setup_vtable () */
  MonoMethod **vtable;

  /* Rarely used fields of classes */
  void *ext;

  void *unity_user_data;
};

struct MonoProperty
{
  MonoClass *parent;
  const char *name;
  MonoMethod *get;
  MonoMethod *set;
  guint32 attrs;
};

struct MonoEvent
{
  MonoClass *parent;
  const char *name;
  MonoMethod *add;
  MonoMethod *remove;
  MonoMethod *raise;
#ifndef MONO_SMALL_CONFIG
  MonoMethod **other;
#endif
  guint32 attrs;
};

typedef struct MonoFieldDefaultValue
{
  /*
   * If the field is constant, pointer to the metadata constant
   * value.
   * If the field has an RVA flag, pointer to the data.
   * Else, invalid.
   */
  const char *data;

  /* If the field is constant, the type of the constant. */
  MonoTypeEnum def_type;
} MonoFieldDefaultValue;

typedef struct
{
  struct
  {
#if MONO_SMALL_CONFIG
    guint16 first, count;
#else
    guint32 first, count;
#endif
  } property, event;

  /* Initialized by a call to mono_class_setup_properties () */
  MonoProperty *properties;

  /* Initialized by a call to mono_class_setup_events () */
  MonoEvent *events;

  guint32 declsec_flags; /* declarative security attributes flags */

  /* Default values/RVA for fields and properties */
  /* Accessed using mono_class_get_field_default_value () / mono_field_get_data () */
  MonoFieldDefaultValue *field_def_values;
  MonoFieldDefaultValue *prop_def_values;

  void *nested_classes;
} MonoClassExt;
