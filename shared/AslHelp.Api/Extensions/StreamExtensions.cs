using System;
using System.Buffers;
using System.IO;
using System.Text;

using AslHelp.Collections.Extensions;

using CommunityToolkit.HighPerformance;

namespace AslHelp.Api.Extensions;

internal static class StreamExtensions
{
    /// <summary>
    ///     Reads an ASCII <see cref="string"/> from the given <see cref="Stream"/>.
    /// </summary>
    /// <param name="stream">
    ///     The <see cref="Stream"/> to read from.
    /// </param>
    /// <returns>
    ///     The ASCII <see cref="string"/> read from the <see cref="Stream"/>.
    /// </returns>
    /// <remarks>
    ///     The <see cref="string"/> is read as follows:
    ///     <list type="number">
    ///         <item>Read the length of the ASCII bytes as an <see cref="int"/>. If this is &lt;= 0, <see cref="string.Empty"/> is returned.</item>
    ///         <item>Read the ASCII bytes.</item>
    ///         <item>Convert the bytes and return the <see cref="string"/>.</item>
    ///     </list>
    /// </remarks>
    public static unsafe string ReadString(this Stream stream)
    {
        int length = stream.Read<int>();
        if (length <= 0)
        {
            return "";
        }

        byte[]? rented = null;
        Span<byte> buffer =
            length <= 256
            ? stackalloc byte[256]
            : rented = ArrayPool<byte>.Shared.Rent(length);

        length = stream.Read(buffer[..length]);

        fixed (byte* pBuffer = buffer)
        {
            string value = Encoding.ASCII.GetString(pBuffer, length);
            ArrayPool<byte>.Shared.ReturnIfNotNull(rented);
            return value;
        }
    }

    /// <summary>
    ///     Writes the given ASCII <see cref="string"/> to the given <see cref="Stream"/>.
    /// </summary>
    /// <param name="stream"></param>
    /// <param name="value"></param>
    /// <remarks>
    ///     The <see cref="string"/> is written as follows:
    ///     <list type="number">
    ///         <item>Write the length of the ASCII bytes as an <see cref="int"/>. If the <see cref="string"/> is <see langword="null"/>, write 0.</item>
    ///         <item>Write the ASCII bytes.</item>
    ///     </list>
    /// </remarks>
    public static unsafe void WriteString(this Stream stream, string? value)
    {
        if (value is null)
        {
            stream.Write(0);
            return;
        }

        int maxLength = Encoding.ASCII.GetMaxByteCount(value.Length);
        if (maxLength <= 0)
        {
            stream.Write(0);
            return;
        }

        byte[]? rented = null;
        Span<byte> buffer =
            maxLength <= 256
            ? stackalloc byte[256]
            : rented = ArrayPool<byte>.Shared.Rent(maxLength);

        fixed (byte* pBuffer = buffer)
        fixed (char* pValue = value)
        {
            int length = Encoding.ASCII.GetBytes(pValue, value.Length, pBuffer, maxLength);

            stream.Write(length);
            stream.Write(buffer[..length]);

            ArrayPool<byte>.Shared.ReturnIfNotNull(rented);
        }
    }
}
