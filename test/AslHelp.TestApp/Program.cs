using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;

using AslHelp.Shared;

using Reloaded.Injector;

const string DllPath = @"D:\Code\Projects\.just-ero\asl-help-v3\artifacts\publish\AslHelp.Native\release_win-x86\AslHelp.Native.dll";

using var game = Process.GetProcessesByName("ElenaTemple").Single();
using var injector = new Injector(game);

var handle = injector.Inject(DllPath);
Console.WriteLine($"Handle: {handle:X}");

var ret = injector.CallFunction(DllPath, NativeApi.StartListener, 0);
Console.WriteLine($"Return: {ret:X}");

using var client = Pipes.Sender;
var endpoint = Connections.SenderEndpoint;

client.Connect(endpoint);

Thread.Sleep(1000);
client.Send([0], 1);

Console.WriteLine("Sent command 0");
System.Console.WriteLine(nameof(nint));

injector.Eject(DllPath);
