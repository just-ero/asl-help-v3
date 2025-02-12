using System;

namespace AslHelp.Ipc.Protocol;

public interface IExitCodeDescriptor<TExitCode>
    where TExitCode : unmanaged, Enum
{
    TExitCode Ok { get; }

    bool IsOk(TExitCode exitCode);
    string GetMessage(TExitCode exitCode);
}

public interface IExitCodeDescriptor<TExitCode, TRequest>
{
    TExitCode Ok { get; }

    bool IsOk(TExitCode exitCode);
    string GetMessage(TExitCode exitCode, TRequest request);
}
