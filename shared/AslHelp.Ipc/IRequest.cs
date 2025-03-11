using AslHelp.Shared.Results;

namespace AslHelp.Ipc;

public interface IRequest<out TResponse, TVisitor>
{
    IResult<TResponse> Visit(TVisitor visitor);
}
