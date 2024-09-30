namespace AslHelp.Api.Requests;

public sealed class GetMonoClassRequest(
    ulong image,
    string @namespace,
    string name) : IRequest
{
    public ulong Image { get; } = image;
    public string Namespace { get; } = @namespace;
    public string Name { get; } = name;

    public RequestCode Code => RequestCode.GetMonoClass;
}
