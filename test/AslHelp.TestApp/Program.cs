using System;
using System.Diagnostics;
using System.Linq;

using AslHelp.Api;
using AslHelp.Api.Clients;
using AslHelp.Api.Requests;
using AslHelp.Memory.Native;

const string DllPath = @"D:\Code\Projects\.just-ero\asl-help-v3\artifacts\publish\AslHelp.Native\release_win-x86\AslHelp.Native.dll";

using Process game = Process.GetProcessesByName("ElenaTemple").Single();

var success = game.Inject(DllPath);

var entryPointReturnValue = game.CallRemoteFunction(DllPath, ApiResourceStrings.ApiEntryPoint, 0);

using var client = new MonoClient(ApiResourceStrings.PipeName);
client.Connect();

AslHelp.Api.Responses.GetMonoImageResponse monoImageResponse = client.GetMonoImage(new("Assembly-CSharp")).Unwrap();
Console.WriteLine(monoImageResponse.Format());

AslHelp.Api.Responses.GetMonoClassResponse monoClassResponse = client.GetMonoClass(new(monoImageResponse.Address, "", "Player")).Unwrap();
Console.WriteLine(monoClassResponse.Format());

client.SendRequest(RequestCode.Close);

game.Eject(DllPath);
