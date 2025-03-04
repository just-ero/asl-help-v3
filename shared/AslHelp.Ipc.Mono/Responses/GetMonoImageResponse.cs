namespace AslHelp.Ipc.Mono.Responses;

public sealed record GetMonoImageResponse : MonoResponseBase
{
    public override void Visit(IMonoResponseVisitor visitor)
    {
        visitor.Accept(this);
    }
}
