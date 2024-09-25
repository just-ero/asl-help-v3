using System;
using System.Buffers;
using System.Buffers.Binary;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text.Json;

using AslHelp.Api.Responses;

namespace AslHelp.Api;

public static class ApiSerializer
{
    public static ResponseCode SendPacket<T>(Stream stream, T request)
        where T : IPacket
    {
        Serialize(stream, request);
        return Deserialize<ResponseCode>(stream);
    }

    public static T? ReceivePacket<T>(Stream stream)
        where T : IPacket
    {
        var request = Deserialize<T>(stream);
        if (request is null)
        {
            Serialize(stream, ResponseCode.NullPacket);
            return default;
        }

        Serialize(stream, ResponseCode.Ok);

        return request;
    }

    public static void Serialize<T>(Stream stream, T value)
    {
        var bytes = JsonSerializer.SerializeToUtf8Bytes(value, typeof(T), ApiSerializerContext.Default);

        var rented = ArrayPool<byte>.Shared.Rent(sizeof(int));
        BinaryPrimitives.WriteInt32LittleEndian(rented, bytes.Length);

        stream.Write(rented, 0, sizeof(int));
        ArrayPool<byte>.Shared.Return(rented);

        stream.Write(bytes, 0, bytes.Length);
    }

    public static T? Deserialize<T>(Stream stream)
    {
        var rented = ArrayPool<byte>.Shared.Rent(sizeof(int));
        if (stream.Read(rented, 0, sizeof(int)) != sizeof(int))
        {
            ThrowEndOfStreamException();
        }

        var length = BinaryPrimitives.ReadInt32LittleEndian(rented);
        ArrayPool<byte>.Shared.Return(rented);

        var buffer = ArrayPool<byte>.Shared.Rent(length);
        if (stream.Read(buffer, 0, length) != length)
        {
            ThrowEndOfStreamException();
        }

        var value = JsonSerializer.Deserialize(buffer.AsSpan(0, length), typeof(T), ApiSerializerContext.Default);
        ArrayPool<byte>.Shared.Return(buffer);

        return (T?)value;
    }

    [DoesNotReturn]
    private static void ThrowEndOfStreamException()
    {
        throw new EndOfStreamException("The stream didn't contain enough data to read the requested item.");
    }
}
