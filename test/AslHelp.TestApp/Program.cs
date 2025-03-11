using System;

using AslHelp.Ipc.Mono;
using AslHelp.Ipc.Mono.Transmission.Commands;
using AslHelp.Shared.Results;

using var monoServer = new DummyMonoServer("asl-help-pipe");
using var monoClient = new MonoClient("asl-help-pipe");

var connection = monoServer.WaitForConnectionAsync();
monoClient.Connect();
await connection;

var process = monoServer.ProcessMessage();
var res = monoClient.GetMonoImage(new("test"));
await process;

Console.WriteLine(res.Value);

public sealed class DummyMonoServer(string pipeName) : MonoServer(pipeName)
{
    public override Result<GetMonoImageResponse> GetMonoImage(GetMonoImageRequest request)
    {
        Console.WriteLine($"Handling {request}...");
        return new GetMonoImageResponse(0, "fart", "poop", "shit");
    }
}
