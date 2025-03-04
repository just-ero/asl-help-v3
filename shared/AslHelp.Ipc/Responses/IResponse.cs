namespace AslHelp.Ipc.Responses;

public interface IResponse<TVisitor>
{
    void Visit(TVisitor visitor);
}
