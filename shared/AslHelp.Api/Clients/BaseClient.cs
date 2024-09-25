using System;
using System.IO.Pipes;

namespace AslHelp.Api.Clients;

public abstract class BaseClient : IDisposable
{
    protected readonly NamedPipeClientStream _pipe;

    protected BaseClient(string pipeName)
        : this(pipeName, PipeOptions.None) { }

    protected BaseClient(string pipeName, PipeOptions options)
    {
        _pipe = new(".", pipeName, PipeDirection.InOut, options);
    }

    public void Connect(int timeout = 0)
    {
        _pipe.Connect(timeout);
    }

    public void Dispose()
    {
        if (_pipe.IsConnected)
        {
            _pipe.Dispose();
        }
    }
}
