namespace AslHelp.Ipc.Requests;

public interface IRequest<TVisitor>
{
    void Visit(TVisitor visitor);
}
