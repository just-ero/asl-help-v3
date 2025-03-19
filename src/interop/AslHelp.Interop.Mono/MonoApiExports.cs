// MonoDomain
global using unsafe mono_get_root_domain = delegate* unmanaged[Cdecl]<nint>;

// MonoImage
global using unsafe mono_image_loaded = delegate* unmanaged[Cdecl]<sbyte*, nint>;
global using unsafe mono_image_get_filename = delegate* unmanaged[Cdecl]<nint, sbyte*>;

// MonoClass
global using unsafe mono_class_get = delegate* unmanaged[Cdecl]<nint, uint, nint>;
global using unsafe mono_class_from_name_case = delegate* unmanaged[Cdecl]<nint, sbyte*, sbyte*, nint>;
global using unsafe mono_class_vtable = delegate* unmanaged[Cdecl]<nint, nint, nint>;
global using unsafe mono_class_get_field_from_name = delegate* unmanaged[Cdecl]<nint, sbyte*, nint>;

// MonoClassField
global using unsafe mono_field_get_type = delegate* unmanaged[Cdecl]<nint, nint>;
global using unsafe mono_field_get_offset = delegate* unmanaged[Cdecl]<nint, uint>;

// MonoType
global using unsafe mono_type_get_name = delegate* unmanaged[Cdecl]<nint, sbyte*>;

// MonoVTable
global using unsafe mono_vtable_get_static_field_data = delegate* unmanaged[Cdecl]<nint, nint>;
