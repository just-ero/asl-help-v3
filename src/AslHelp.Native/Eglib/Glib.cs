namespace AslHelp.Native.Eglib;

/// <remarks>
///     <see href="https://github.com/Unity-Technologies/mono/blob/unity-2017.1-mbe/eglib/src/glib.h#L306-L309"/>
/// </remarks>
internal unsafe struct GSList
{
    public void* Data;
    public GSList* Next;
}
