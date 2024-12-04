namespace AslHelp.Api.Requests;

public sealed class GetMonoImageRequest(
    string name) : IRequest
{
    public string Name { get; } = name;

    public RequestCode Code => RequestCode.GetMonoImage;
}
