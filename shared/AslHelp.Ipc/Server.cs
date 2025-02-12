using System;
using System.IO.Pipes;

namespace AslHelp.Ipc;

public abstract class Server : IDisposable
{
    private readonly NamedPipeServerStream _pipe;

    public Server(string pipeName)
    {
        _pipe = new(pipeName, PipeDirection.InOut, 1, PipeTransmissionMode.Byte, PipeOptions.Asynchronous);
    }

    public void Dispose()
    {
        _pipe.Dispose();
    }
}
