using System;
using System.IO.Pipes;

namespace AslHelp.Api.Servers;

public abstract class BaseServer : IDisposable
{
    protected readonly NamedPipeServerStream _pipe;

    protected BaseServer(string pipeName)
        : this(pipeName, PipeOptions.None) { }

    protected BaseServer(string pipeName, PipeOptions options)
    {
        _pipe = new(pipeName, PipeDirection.InOut, 1, PipeTransmissionMode.Byte, options);
    }

    public void WaitForConnection()
    {
        _pipe.WaitForConnection();
    }

    public void Dispose()
    {
        if (_pipe.IsConnected)
        {
            _pipe.Dispose();
        }
    }
}
