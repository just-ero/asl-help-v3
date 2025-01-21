using System;
using System.Diagnostics;
using System.Linq;

using AslHelp.Ipc;
using AslHelp.Ipc.Clients;
using AslHelp.Ipc.Requests;
using AslHelp.Memory.Extensions;

const string DllPath = @"D:\Code\Projects\.just-ero\asl-help-v3\artifacts\publish\AslHelp.Native\debug_win-x86\AslHelp.Native.dll";

using Process game = Process.GetProcessesByName("ElenaTemple").Single();

var dll = game.Inject(DllPath).Unwrap();
uint ret = dll.CallRemoteFunction(ApiResourceStrings.ApiEntryPoint, 0).Unwrap();

using var client = new MonoClient(ApiResourceStrings.PipeName);
client.Connect();

var image = client.GetMonoImage("Assembly-CSharp").Unwrap();
Console.WriteLine(image);

var klass = client.GetMonoClass(image.Address, "Player").Unwrap();
Console.WriteLine(klass);

// var fields = client.GetMonoClassFields(klass.Address).Unwrap();
// foreach (var field in fields)
// {
//     Console.WriteLine(field.Unwrap());
//     if (field.Unwrap().Name == "deathSound")
//     {
//         break;
//     }
// }

client.SendRequest(RequestCode.Close);

dll.Eject();
