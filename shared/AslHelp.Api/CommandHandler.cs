using System.IO;

namespace AslHelp.Api;

public static class CommandHandler
{
    public static TOut Handle<TIn, TOut>(this Stream stream, TIn input)
        where TIn : IApiPacket<TIn>
        where TOut : IApiPacket<TOut>, new()
    {
        input.Write(stream);
        return new TOut().Read(stream);
    }

    public static TOut Handle<TIn, TOut, TCmd>(this Stream stream, TIn input)
        where TCmd : ICommand<TIn, TOut>, new()
    {

    }
}
