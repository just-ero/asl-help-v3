using System.IO;

namespace AslHelp.Shared.IO;

public interface IPipePacket<TSelf>
    where TSelf : IPipePacket<TSelf>
{
    void Send(Stream stream);
    TSelf Receive(Stream stream);
}
