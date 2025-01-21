using System;

using AslHelp.Shared.Results.Errors;

namespace AslHelp.Ipc.Protocol;

public interface IRequest<TExitCode>
    where TExitCode : unmanaged, Enum
{
    IResultError GetErr(TExitCode code);
}
