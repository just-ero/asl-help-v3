using System;
using System.Buffers;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;

using AslHelp.Shared.Extensions;

using CommunityToolkit.HighPerformance;

namespace AslHelp.Shared.IO.Requests;

public sealed class MonoClassRequest : IPipePacket<MonoClassRequest>
{
#pragma warning disable CS8618 // Non-nullable property must contain a non-null value when exiting constructor.
    [Obsolete($"Use {nameof(MonoClassRequest)}(string, string).", true)]
    public MonoClassRequest() { }
#pragma warning restore CS8618

    public MonoClassRequest(string @namespace, string name)
    {
        Namespace = @namespace;
        Name = name;
    }

    public string Namespace { get; private set; }
    public string Name { get; private set; }

    MonoClassRequest IPipePacket<MonoClassRequest>.Receive(Stream stream)
    {
        Namespace = stream.ReadString();
        Name = stream.ReadString();
        return this;
    }

    void IPipePacket<MonoClassRequest>.Send(Stream stream)
    {
        stream.WriteString(Namespace);
        stream.WriteString(Name);
    }
}
