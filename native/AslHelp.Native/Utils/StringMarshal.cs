using System;
using System.Runtime.InteropServices;
using System.Text;

namespace AslHelp.Native.Utils;

internal static class StringMarshal
{
    public static unsafe string CreateStringFromNullTerminated(sbyte* bytes)
    {
        ReadOnlySpan<byte> span = MemoryMarshal.CreateReadOnlySpanFromNullTerminated((byte*)bytes);
        return Encoding.UTF8.GetString(span);
    }
}
