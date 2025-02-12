using System;
using System.IO.Pipes;

using AslHelp.Ipc.Protocol;
using AslHelp.Ipc.Responses;
using AslHelp.Shared.Results;
using AslHelp.Shared.Results.Errors;

namespace AslHelp.Ipc;

public abstract class Server<TCommand> : IDisposable
    where TCommand : unmanaged, Enum
{
    private readonly NamedPipeServerStream _pipe;

    public Server(string pipeName)
        : this(pipeName, PipeOptions.None) { }

    public Server(string pipeName, PipeOptions options)
    {
        _pipe = new(pipeName, PipeDirection.InOut, 1, PipeTransmissionMode.Byte, options);
    }

    public void WaitForConnection()
    {
        _pipe.WaitForConnection();
    }

    public void WaitForCommand()
    {
        TCommand cmd = IpcSerializer.Deserialize<TCommand>(_pipe);

        IResult result;
        try
        {
            result = HandleCommand(cmd);
        }
        catch (Exception ex)
        {
            result = Result.Err(new ExceptionError(ex));
        }

        if (result.IsOk)
        {
            IpcSerializer.Serialize(_pipe, true);

            if (result is IResult<IResponse> { Value: var value })
            {
                IpcSerializer.Serialize(_pipe, value);
            }
        }
        else
        {
            IpcSerializer.Serialize(_pipe, false);
            IpcSerializer.Serialize(_pipe, $"[{cmd}] {result.Error}");
        }
    }

    protected abstract IIpcResult HandleCommand(TCommand command);

    public void Dispose()
    {
        _pipe.Dispose();
    }
}
