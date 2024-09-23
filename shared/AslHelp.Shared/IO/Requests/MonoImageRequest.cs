using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;

using AslHelp.Shared.Extensions;

namespace AslHelp.Shared.IO.Requests;

public sealed class MonoImageRequest : IPipePacket<MonoImageRequest>
{
#pragma warning disable CS8618 // Non-nullable property must contain a non-null value when exiting constructor.
    [Obsolete($"Use {nameof(MonoImageRequest)}(string).", true)]
    public MonoImageRequest() { }
#pragma warning restore CS8618

    public MonoImageRequest(string nameOrPath)
    {
        NameOrPath = nameOrPath;
    }

    public string NameOrPath { get; private set; }

    [MemberNotNull(nameof(NameOrPath))]
    unsafe MonoImageRequest IPipePacket<MonoImageRequest>.Receive(Stream stream)
    {
        NameOrPath = stream.ReadString();
        return this;
    }

    unsafe void IPipePacket<MonoImageRequest>.Send(Stream stream)
    {
        stream.WriteString(NameOrPath);
    }
}
