using System.IO.Pipes;

namespace AslHelp.Api.Servers;

public sealed class MonoServer : BaseServer
{
    public MonoServer()
        : base(ApiResourceStrings.PipeName) { }

    public MonoServer(string pipeName)
        : base(pipeName, PipeOptions.None) { }

    public MonoServer(PipeOptions options)
        : base(ApiResourceStrings.PipeName, options) { }

    public MonoServer(string pipeName, PipeOptions options)
        : base(pipeName, options) { }

    public Requests.RequestCode NextRequestCode()
    {
        return ApiSerializer.Deserialize<Requests.RequestCode>(_pipe);
    }
}
