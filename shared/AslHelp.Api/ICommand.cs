using System.IO;

namespace AslHelp.Api;

public interface ICommand<TIn, TOut>
{
    IApiPacket<TOut> Request { get; }
}

public interface IRequest<in TRequest, out TResponse>
{
    void Write(Stream stream, TRequest request);
    TResponse Read(Stream stream);
}

public interface IResponse<TResponse>
{
    void Write(Stream stream, TResponse response);
    TResponse Read(Stream stream);
}

public class ImgCmd : ICommand<string, ulong>
{

}
