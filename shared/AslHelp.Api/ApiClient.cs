using System.IO;
using System.IO.Pipes;

namespace AslHelp.Api;

public sealed class ApiClient : Stream
{
    private readonly NamedPipeClientStream _client;

    public override bool CanWrite => _client.CanWrite;
    public override bool CanRead => _client.CanRead;
    public override bool CanSeek => _client.CanSeek;

    public override long Length => _client.Length;
    public override long Position { get => _client.Position; set => _client.Position = value; }

    public ApiClient(string pipeName)
    {
        _client = new(".", pipeName, PipeDirection.InOut, PipeOptions.Asynchronous);
    }

    public void Write<T>(T packet)
        where T : IApiPacket<T>
    {
        packet.Write(_client);
    }

    public T Read<T>()
        where T : IApiPacket<T>, new()
    {
        return new T().Read(_client);
    }

    public override void Write(byte[] buffer, int offset, int count)
    {
        _client.Write(buffer, offset, count);
    }

    public override int Read(byte[] buffer, int offset, int count)
    {
        return _client.Read(buffer, offset, count);
    }

    public override long Seek(long offset, SeekOrigin origin)
    {
        return _client.Seek(offset, origin);
    }

    public override void SetLength(long value)
    {
        _client.SetLength(value);
    }

    public override void Flush()
    {
        _client.Flush();
    }

    public void Connect()
    {
        _client.Connect();
    }

    public override void Close()
    {
        _client.Close();
    }
}
