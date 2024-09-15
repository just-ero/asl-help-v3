using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;

namespace AslHelp.Shared.IO.Requests;

public sealed class MonoClassRequest : IPipeTransaction<MonoClassRequest>
{
    public string? Namespace { get; private set; }
    public string? Name { get; private set; }

    [MemberNotNull(nameof(Namespace), nameof(Name))]
    public MonoClassRequest Receive(Stream stream)
    {
        int length = stream.Read<int>();

        byte[] buffer = new byte[length];
        stream.Read(buffer, 0, length);

        Namespace = Encoding.ASCII.GetString(buffer);

        length = stream.Read<int>();

        buffer = new byte[length];
        stream.Read(buffer, 0, length);

        Name = Encoding.ASCII.GetString(buffer);

        return this;
    }

    public void Send(Stream stream)
    {
        byte[] buffer = Encoding.ASCII.GetBytes(Namespace);

        stream.Write(buffer.Length);
        stream.Write(buffer, 0, buffer.Length);

        buffer = Encoding.ASCII.GetBytes(Name);

        stream.Write(buffer.Length);
        stream.Write(buffer, 0, buffer.Length);
    }
}
