using AslHelp.Shared.Results;

namespace AslHelp.Ipc;

public interface IRequest<out TResponse, TVisitor>
{
    IResult<TResponse> Handle(TVisitor visitor);
}
