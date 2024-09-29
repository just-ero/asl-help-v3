using System;
using System.Diagnostics;
using System.Linq;

using AslHelp.Api;
using AslHelp.Api.Clients;
using AslHelp.Api.Requests;
using AslHelp.Api.Responses;
using AslHelp.Memory;
using AslHelp.Memory.Extensions;

const string DllPath = @"D:\Code\Projects\.just-ero\asl-help-v3\artifacts\publish\AslHelp.Native\release_win-x86\AslHelp.Native.dll";

using Process game = Process.GetProcessesByName("ElenaTemple").Single();

Module dll = game.Inject(DllPath).Unwrap();
uint ret = dll.CallRemoteFunction(ApiResourceStrings.ApiEntryPoint, 0).Unwrap();

using MonoClient client = new(ApiResourceStrings.PipeName);
client.Connect();

GetMonoImageResponse monoImageResponse = client.GetMonoImage(new("Assembly-CSharp")).Unwrap();
Console.WriteLine(monoImageResponse.Format());

GetMonoClassResponse monoClassResponse = client.GetMonoClass(new(monoImageResponse.Address, "", "Player")).Unwrap();
Console.WriteLine(monoClassResponse.Format());

client.SendRequest(RequestCode.Close);

dll.Eject();
