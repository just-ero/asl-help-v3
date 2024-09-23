using System;
using System.IO;

using CommunityToolkit.HighPerformance;

namespace AslHelp.Shared.IO.Responses;

public sealed class MonoImageResponse : IPipePacket<MonoImageResponse>
{
    [Obsolete($"Use {nameof(MonoImageResponse)}(nint).", true)]
    public MonoImageResponse() { }

    public MonoImageResponse(nuint handle)
    {
        Handle = handle;
    }

    public ulong Handle { get; private set; }

    void IPipePacket<MonoImageResponse>.Send(Stream stream)
    {
        stream.Write(Handle);
    }

    MonoImageResponse IPipePacket<MonoImageResponse>.Receive(Stream stream)
    {
        Handle = stream.Read<ulong>();
        return this;
    }
}
