namespace AslHelp.Ipc.Mono.Requests;

public sealed record GetMonoImageRequest(
    string Name) : MonoRequestBase
{
    public override void Visit(IMonoRequestVisitor visitor)
    {
        visitor.Handle(this);
    }
}
