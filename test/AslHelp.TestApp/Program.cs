using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

using AslHelp.Ipc.Mono;
using AslHelp.Memory.Extensions;
using AslHelp.Native.Interop;
using AslHelp.Native.Interop.Commands;

const string DllPath = @"C:\Users\Ero\Desktop\code\.just-ero\asl-help-v3\artifacts\publish\AslHelp.Native\debug_win-x86\AslHelp.Native.dll";

using var game = Process.GetProcessesByName("ElenaTemple")[0];

var dll = game.Inject(DllPath).Unwrap();
var ret = dll.CallRemoteFunction(Cmd.StartServer, StartServerRequest.StartMonoServer)
    .Map(code => Unsafe.As<uint, StartServerResponse>(ref code))
    .Unwrap();

Console.WriteLine(ret);

using var client = new MonoClient("asl-help-ipc");
client.Connect(1000);

var image = client.GetMonoImage("Assembly-CSharp").Unwrap();
Console.WriteLine(image);

var klass = client.GetMonoClass(image.Address, "", "GameProgress").Unwrap();
Console.WriteLine(klass);

var field = client.GetMonoField(klass.Address, "instance").Unwrap();
Console.WriteLine(field);

dll.Eject();
