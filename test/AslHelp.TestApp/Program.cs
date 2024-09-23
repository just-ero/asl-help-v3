using System;
using System.Diagnostics;
using System.IO.Pipes;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

using AslHelp.Api;
using AslHelp.Api.Requests;
using AslHelp.Api.Responses;

using Reloaded.Injector;

const string DllPath = @"D:\Code\Projects\.just-ero\asl-help-v3\artifacts\publish\AslHelp.Native\release_win-x86\AslHelp.Native.dll";

Console.WriteLine("Connecting to game...");

using var game = Process.GetProcessesByName("ElenaTemple").Single();

Console.WriteLine($"  => Connected to game: {game.ProcessName} ({game.Id})");
Console.WriteLine("Injecting...");

using var injector = new Injector(game);
var handle = injector.Inject(DllPath);

Console.WriteLine($"  => Injected: {handle:X}");
Console.WriteLine($"Calling {ApiResourceStrings.ApiEntryPoint}...");

var entryPointReturnValue = injector.CallFunction(DllPath, ApiResourceStrings.ApiEntryPoint, 0);

Console.WriteLine($"  => {ApiResourceStrings.ApiEntryPoint} returned: {entryPointReturnValue}");
Console.WriteLine("Connecting to named pipe...");

using var pipe = new NamedPipeClientStream(".", "asl-help");
await pipe.ConnectAsync();

Console.WriteLine("  => Connected.");
Console.WriteLine($"Sending request: {RequestCode.None}");

await serialize(RequestCode.None);

Console.WriteLine($"Sending request: {RequestCode.Close}");

await serialize(RequestCode.Close);
var response = await deserialize<ResponseCode>();

Console.WriteLine($"  => Received response: {response}");

injector.Eject(DllPath);

async Task<T?> deserialize<T>()
{
    return (T?)await JsonSerializer.DeserializeAsync(pipe, typeof(T), ApiSerializerContext.Default);
}

Task serialize<T>(T value)
{
    return JsonSerializer.SerializeAsync(pipe, value, typeof(T), ApiSerializerContext.Default);
}
