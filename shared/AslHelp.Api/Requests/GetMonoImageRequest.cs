namespace AslHelp.Api.Requests;

public sealed class GetMonoImageRequest(
    string fileName) : IRequest
{
    public string FileName { get; } = fileName;

    public RequestCode Code => RequestCode.GetMonoImage;
}
