// This file is licensed under the MIT License.

using System;
using System.Buffers;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace AslHelp.Shared.IO;

internal static class StreamExtensions
{
    public static Task<int> ReadAsync(this Stream stream, Memory<byte> buffer, CancellationToken cancellationToken = default)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            return Task.FromCanceled<int>(cancellationToken);
        }

        if (MemoryMarshal.TryGetArray(buffer, out ArraySegment<byte> segment))
        {
            return stream.ReadAsync(segment.Array!, segment.Offset, segment.Count, cancellationToken);
        }

        return readAsyncFallback(stream, buffer, cancellationToken);

        static async Task<int> readAsyncFallback(Stream stream, Memory<byte> buffer, CancellationToken cancellationToken)
        {
            byte[] rent = ArrayPool<byte>.Shared.Rent(buffer.Length);

            try
            {
                int bytesRead = await stream.ReadAsync(rent, 0, buffer.Length, cancellationToken).ConfigureAwait(false);

                if (bytesRead > 0)
                {
                    rent.AsSpan(0, bytesRead).CopyTo(buffer.Span);
                }

                return bytesRead;
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(rent);
            }
        }
    }

    public static Task WriteAsync(this Stream stream, ReadOnlyMemory<byte> buffer, CancellationToken cancellationToken = default)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            return Task.FromCanceled(cancellationToken);
        }

        if (MemoryMarshal.TryGetArray(buffer, out ArraySegment<byte> segment))
        {
            return stream.WriteAsync(segment.Array!, segment.Offset, segment.Count, cancellationToken);
        }

        return writeAsyncFallback(stream, buffer, cancellationToken);

        static async Task writeAsyncFallback(Stream stream, ReadOnlyMemory<byte> buffer, CancellationToken cancellationToken)
        {
            byte[] rent = ArrayPool<byte>.Shared.Rent(buffer.Length);

            try
            {
                buffer.Span.CopyTo(rent);

                await stream.WriteAsync(rent, 0, buffer.Length, cancellationToken).ConfigureAwait(false);
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(rent);
            }
        }
    }

    public static int Read(this Stream stream, Span<byte> buffer)
    {
        byte[] rent = ArrayPool<byte>.Shared.Rent(buffer.Length);

        try
        {
            int bytesRead = stream.Read(rent, 0, buffer.Length);

            if (bytesRead > 0)
            {
                rent.AsSpan(0, bytesRead).CopyTo(buffer);
            }

            return bytesRead;
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(rent);
        }
    }

    public static void Write(this Stream stream, ReadOnlySpan<byte> buffer)
    {
        byte[] rent = ArrayPool<byte>.Shared.Rent(buffer.Length);

        try
        {
            buffer.CopyTo(rent);

            stream.Write(rent, 0, buffer.Length);
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(rent);
        }
    }

    public static unsafe T Read<T>(this Stream stream)
        where T : unmanaged
    {
        int bytesOffset = 0;
        byte[] buffer = ArrayPool<byte>.Shared.Rent(sizeof(T));

        try
        {
            do
            {
                int bytesRead = stream.Read(buffer, bytesOffset, sizeof(T) - bytesOffset);

                if (bytesRead == 0)
                {
                    ThrowEndOfStreamException();
                }

                bytesOffset += bytesRead;
            }
            while (bytesOffset < sizeof(T));

            return Unsafe.ReadUnaligned<T>(ref buffer[0]);
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(buffer);
        }
    }

    public static unsafe void Write<T>(this Stream stream, in T value)
        where T : unmanaged
    {
        int length = sizeof(T);
        byte[] buffer = ArrayPool<byte>.Shared.Rent(length);

        try
        {
            Unsafe.WriteUnaligned(ref buffer[0], value);

            stream.Write(buffer, 0, length);
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(buffer);
        }
    }

    private static void ThrowEndOfStreamException()
    {
        throw new EndOfStreamException("The stream didn't contain enough data to read the requested item.");
    }
}
