namespace AslHelp.Api.Requests;

public interface IRequest : IPayload
{
    RequestCode Code { get; }
}
