using System.IO;

namespace AslHelp.Shared.IO;

public static class TransactionExtensions
{
    public static void Send<T>(this Stream stream, T value)
        where T : IPipeTransaction<T>
    {
        value.Send(stream);
    }

    public static T Receive<T>(this Stream stream)
        where T : IPipeTransaction<T>, new()
    {
        T value = new T();
        value.Receive(stream);
        return value;
    }
}
