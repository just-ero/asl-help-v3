using System;
using System.Buffers;
using System.Buffers.Binary;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

using AslHelp.Shared;

namespace AslHelp.Ipc.Serialization;

internal static class IpcSerializer
{
    public static void Serialize<T>(Stream stream, T value, JsonSerializerContext context)
    {
        byte[] bytes = JsonSerializer.SerializeToUtf8Bytes(value, typeof(T), context);

        byte[] rented = ArrayPool<byte>.Shared.Rent(sizeof(int));
        BinaryPrimitives.WriteInt32LittleEndian(rented, bytes.Length);

        stream.Write(rented, 0, sizeof(int));
        stream.Write(bytes, 0, bytes.Length);

        ArrayPool<byte>.Shared.Return(rented);
    }

    public static async Task SerializeAsync<T>(Stream stream, T value, JsonSerializerContext context, CancellationToken ct)
    {
        byte[] bytes = JsonSerializer.SerializeToUtf8Bytes(value, typeof(T), context);

        byte[] rented = ArrayPool<byte>.Shared.Rent(sizeof(int));
        BinaryPrimitives.WriteInt32LittleEndian(rented, bytes.Length);

        await stream.WriteAsync(rented, 0, sizeof(int), ct);
        await stream.WriteAsync(bytes, 0, bytes.Length, ct);

        ArrayPool<byte>.Shared.Return(rented);
    }

    public static T? Deserialize<T>(Stream stream, JsonSerializerContext context)
    {
        byte[] rented = ArrayPool<byte>.Shared.Rent(sizeof(int));
        if (stream.Read(rented, 0, sizeof(int)) != sizeof(int))
        {
            const string Msg = "The stream didn't contain enough data to read the requested item.";
            ThrowHelper.ThrowEndOfStreamException(Msg);
        }

        int length = BinaryPrimitives.ReadInt32LittleEndian(rented);
        ArrayPool<byte>.Shared.Return(rented);

        byte[] buffer = ArrayPool<byte>.Shared.Rent(length);
        if (stream.Read(buffer, 0, length) != length)
        {
            const string Msg = "The stream didn't contain enough data to read the requested item.";
            ThrowHelper.ThrowEndOfStreamException(Msg);
        }

        object? value = JsonSerializer.Deserialize(buffer.AsSpan(0, length), typeof(T), context);
        ArrayPool<byte>.Shared.Return(buffer);

        return (T?)value;
    }

    public static async Task<T?> DeserializeAsync<T>(Stream stream, JsonSerializerContext context, CancellationToken ct)
    {
        byte[] rented = ArrayPool<byte>.Shared.Rent(sizeof(int));
        if (await stream.ReadAsync(rented, 0, sizeof(int), ct) != sizeof(int))
        {
            const string Msg = "The stream didn't contain enough data to read the requested item.";
            ThrowHelper.ThrowEndOfStreamException(Msg);
        }

        int length = BinaryPrimitives.ReadInt32LittleEndian(rented);
        ArrayPool<byte>.Shared.Return(rented);

        byte[] buffer = ArrayPool<byte>.Shared.Rent(length);
        if (await stream.ReadAsync(buffer, 0, length, ct) != length)
        {
            const string Msg = "The stream didn't contain enough data to read the requested item.";
            ThrowHelper.ThrowEndOfStreamException(Msg);
        }

        object? value = JsonSerializer.Deserialize(buffer.AsSpan(0, length), typeof(T), context);
        ArrayPool<byte>.Shared.Return(buffer);

        return (T?)value;
    }
}
