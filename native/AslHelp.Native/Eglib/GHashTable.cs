namespace AslHelp.Native.Eglib;

/// <remarks>
///     <see href="https://github.com/Unity-Technologies/mono/blob/unity-2017.1-mbe/eglib/src/ghashtable.c#L34-L38"/>
/// </remarks>
internal unsafe struct Slot
{
    public void* Key;
    public void* Value;
    public Slot* Next;
}

/// <remarks>
///     <see href="https://github.com/Unity-Technologies/mono/blob/unity-2017.1-mbe/eglib/src/ghashtable.c#L42-L52"/>
/// </remarks>
internal unsafe struct GHashTable
{
    public delegate* unmanaged[Cdecl]<void*, uint> HashFunc;
    public delegate* unmanaged[Cdecl]<void*, void*, int> KeyEqualFunc;

    public Slot** Table;
    public int TableSize;
    public int InUse;
    public int Threshold;
    public int LastRehash;

    public delegate* unmanaged[Cdecl]<void*, void> ValueDestroyFunc;
    public delegate* unmanaged[Cdecl]<void*, void> KeyDestroyFunc;
}
