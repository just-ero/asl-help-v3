using System;
using System.ComponentModel;
using System.Diagnostics;

using AslHelp.Engines;
using AslHelp.Ipc;
using AslHelp.Ipc.Mono;
using AslHelp.Memory.Extensions;

const string DllPath = @"D:\Code\Projects\.just-ero\asl-help-v3\artifacts\publish\AslHelp.Ipc.Native\debug_win-x86\AslHelp.Ipc.Native.dll";

using var game = Process.GetProcessesByName("ElenaTemple")[0];

var dll = game.Inject(DllPath).Unwrap();
var ret = dll.CallRemoteFunction(IpcConnection.EntryPoint, Engine.Mono).Unwrap();
Console.WriteLine(ret);

using var client = new MonoClient(IpcConnection.PipeName);
client.Connect();

var image = client.GetMonoImage("Assembly-CSharp").Unwrap();
Console.WriteLine(image);

var klass = client.GetMonoClass(image.Address, "", "GameProgress").Unwrap();
Console.WriteLine(klass);

var field = client.GetMonoField(klass.Address, "instance").Unwrap();
Console.WriteLine(field);

unsafe
{
    uint v;
    if (game.ReadMemory((nuint)klass.StaticFieldData + field.Offset, &v, sizeof(uint)))
    {
        Console.WriteLine(v.ToString("X"));
    }
    else
    {
        Console.WriteLine(new Win32Exception());
    }
}

dll.Eject();
