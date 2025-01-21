namespace AslHelp.Ipc.Requests;

public sealed class GetMonoImageRequest(
    string name) : IRequest
{
    public string Name { get; } = name;
}
