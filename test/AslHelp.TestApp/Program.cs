using System;
using System.Diagnostics;

using AslHelp.Ipc;
using AslHelp.Ipc.Mono;
using AslHelp.Memory.Extensions;

const string DllPath = @"C:\Users\Ero\Desktop\code\.just-ero\asl-help-v3\artifacts\publish\AslHelp.Ipc.Native\debug_win-x86\AslHelp.Ipc.Native.dll";

using var game = Process.GetProcessesByName("ElenaTemple")[0];

var dll = game.Inject(DllPath).Unwrap();
var ret = dll.CallRemoteFunction(IpcConnection.EntryPoint, 0).Unwrap();
Console.WriteLine(ret);

using var client = new MonoClient(IpcConnection.PipeName);
client.Connect();

var image = client.GetMonoImage("Assembly-CSharp").Unwrap();
Console.WriteLine(image);

var klass = client.GetMonoClass(image.Address, "", "Player").Unwrap();
Console.WriteLine(klass);

dll.Eject();
