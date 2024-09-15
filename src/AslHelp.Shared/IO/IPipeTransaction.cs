using System.IO;

namespace AslHelp.Shared.IO;

public interface IPipeTransaction<TSelf>
    where TSelf : IPipeTransaction<TSelf>
{
    void Send(Stream stream);
    TSelf Receive(Stream stream);
}
