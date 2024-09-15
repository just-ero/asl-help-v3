using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;

namespace AslHelp.Shared.IO.Requests;

public sealed class MonoImageRequest : IPipeTransaction<MonoImageRequest>
{
    public string? NameOrPath { get; private set; }

    [MemberNotNull(nameof(NameOrPath))]
    public MonoImageRequest Receive(Stream stream)
    {
        int length = stream.Read<int>();

        byte[] buffer = new byte[length];
        stream.Read(buffer, 0, length);

        NameOrPath = Encoding.ASCII.GetString(buffer);

        return this;
    }

    public void Send(Stream stream)
    {
        byte[] buffer = Encoding.ASCII.GetBytes(NameOrPath);

        stream.Write(buffer.Length);
        stream.Write(buffer, 0, buffer.Length);
    }
}
