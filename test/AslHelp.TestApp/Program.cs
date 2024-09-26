using System;
using System.Diagnostics;
using System.Linq;

using AslHelp.Api;
using AslHelp.Api.Clients;
using AslHelp.Api.Requests;
using AslHelp.Memory.Native;

using Reloaded.Injector;

const string DllPath = @"D:\Code\Projects\.just-ero\asl-help-v3\artifacts\publish\AslHelp.Native\release_win-x86\AslHelp.Native.dll";

using var game = Process.GetProcessesByName("ElenaTemple").Single();

var handle = game.Inject(DllPath);
Console.WriteLine($"{handle:X}");

// using var injector = new Injector(game);
// var handle = injector.Inject(DllPath);

// var entryPointReturnValue = injector.CallFunction(DllPath, ApiResourceStrings.ApiEntryPoint, 0);

// using var client = new MonoClient(ApiResourceStrings.PipeName);
// client.Connect();

// var monoImageResponse = client.GetMonoImage(new("Assembly-CSharp")).Unwrap();
// Console.WriteLine(monoImageResponse.Format());

// var monoClassResponse = client.GetMonoClass(new(monoImageResponse.Address, "", "Player")).Unwrap();
// Console.WriteLine(monoClassResponse.Format());

// client.SendRequest(RequestCode.Close);

// injector.Eject(DllPath);
