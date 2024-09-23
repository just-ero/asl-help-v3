using System.IO;

namespace AslHelp.Api;

public interface IApiPacket<TSelf>
    where TSelf : IApiPacket<TSelf>
{
    void Write(Stream stream);
    TSelf Read(Stream stream);
}
