using System;

namespace AslHelp.Ipc.Protocol;

public interface IRequest<TExitCode>
    where TExitCode : unmanaged, Enum
{
    EndpointError GetErr(TExitCode code);
}
