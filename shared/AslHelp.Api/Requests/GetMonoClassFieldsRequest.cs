namespace AslHelp.Api.Requests;

public sealed class GetMonoClassFieldsRequest(
    ulong klass) : IRequest
{
    public ulong Klass { get; } = klass;

    public RequestCode Code => RequestCode.GetMonoClassFields;
}
