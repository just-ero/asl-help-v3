using System;
using System.IO;

using AslHelp.Shared.IO;

namespace AslHelp.Shared.Api.Requests;

public sealed class MonoImageRequest(
    string @namespace,
    string name) : IPipePacket<MonoImageRequest>
{
    [Obsolete($"Use {nameof(MonoImageRequest)}(string, string).", true)]
    public MonoImageRequest()
        : this(null!, null!) { }

    public string Namespace { get; private set; } = @namespace;
    public string Name { get; private set; } = name;

    MonoImageRequest IPipePacket<MonoImageRequest>.Receive(Stream stream)
    {
        Namespace = stream.ReadString();
        Name = stream.ReadString();
        return this;
    }

    void IPipePacket<MonoImageRequest>.Send(Stream stream)
    {
        stream.WriteString(Namespace);
        stream.WriteString(Name);
    }
}
