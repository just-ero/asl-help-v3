using System;

using AslHelp.Ipc.Mono;
using AslHelp.Ipc.Mono.Requests;
using AslHelp.Ipc.Mono.Responses;

using var monoServer = new DummyMonoServer("asl-help-pipe");
using var monoClient = new MonoClient("asl-help-pipe");

var connection = monoServer.WaitForConnectionAsync();
monoClient.Connect();
await connection;

var process = monoServer.ProcessMessage();
monoClient.GetMonoImage("test");
await process;

Console.WriteLine("Done.");

public sealed class DummyMonoServer(string pipeName) : MonoServer(pipeName)
{
    public override GetMonoImageResponse Handle(GetMonoImageRequest request)
    {
        Console.WriteLine($"Handling {request}...");
        return new GetMonoImageResponse();
    }
}
