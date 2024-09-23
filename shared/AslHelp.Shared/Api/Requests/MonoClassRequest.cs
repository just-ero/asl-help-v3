using System;
using System.IO;

using AslHelp.Shared.Extensions;
using AslHelp.Shared.IO;

namespace AslHelp.Shared.Api.Requests;

public sealed class MonoClassRequest(
    string @namespace,
    string name) : IPipePacket<MonoClassRequest>
{
    [Obsolete($"Use {nameof(MonoClassRequest)}(string, string).", true)]
    public MonoClassRequest()
        : this(null!, null!) { }

    public string Namespace { get; private set; } = @namespace;
    public string Name { get; private set; } = name;

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
