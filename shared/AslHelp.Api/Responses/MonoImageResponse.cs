using System;
using System.IO;

using CommunityToolkit.HighPerformance;

namespace AslHelp.Api.Responses;

public sealed class MonoImageResponse(
    ulong address) : IApiPacket<MonoImageResponse>
{
    [Obsolete("For serialization purposes only.", true)]
    public MonoImageResponse()
        : this(0) { }

    public ulong Address { get; private set; } = address;

    void IApiPacket<MonoImageResponse>.Write(Stream stream)
    {
        stream.Write(Address);
    }

    MonoImageResponse IApiPacket<MonoImageResponse>.Read(Stream stream)
    {
        Address = stream.Read<ulong>();

        return this;
    }
}
