namespace AslHelp.Api.Requests;

public sealed class GetMonoImageRequest(
    string name) : IRequest
{
    public string Name { get; } = name;

    RequestCode IRequest.Code => RequestCode.GetMonoImage;
}
