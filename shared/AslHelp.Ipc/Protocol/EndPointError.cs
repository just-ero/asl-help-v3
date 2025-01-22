using AslHelp.Shared.Results.Errors;

namespace AslHelp.Ipc.Protocol;

public abstract record EndpointError : ResultError
{
    protected EndpointError(string message)
        : base(message) { }
}
