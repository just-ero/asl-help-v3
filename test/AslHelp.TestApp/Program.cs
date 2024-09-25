using System;
using System.Diagnostics;
using System.IO.Pipes;
using System.Linq;

using AslHelp.Api;
using AslHelp.Api.Requests;
using AslHelp.Api.Responses;

using Reloaded.Injector;

using static AslHelp.Api.ApiSerializer;

const string DllPath = @"D:\Code\Projects\.just-ero\asl-help-v3\artifacts\publish\AslHelp.Native\release_win-x86\AslHelp.Native.dll";

using var game = Process.GetProcessesByName("ElenaTemple").Single();

using var injector = new Injector(game);
var handle = injector.Inject(DllPath);

var entryPointReturnValue = injector.CallFunction(DllPath, ApiResourceStrings.ApiEntryPoint, 0);

using var pipe = new NamedPipeClientStream(".", "asl-help");
pipe.Connect();

SendPacket(pipe, RequestCode.GetMonoImage, new GetMonoImageRequest("Assembly-CSharp"));
var monoImage = ReceivePacket<GetMonoImageResponse>(pipe);

Console.WriteLine(monoImage);

SendPacket(pipe, RequestCode.GetMonoClass, new GetMonoClassRequest(monoImage!.Address, "", "Player"));
var monoClass = ReceivePacket<GetMonoClassResponse>(pipe);

Console.WriteLine(monoClass);

Serialize(pipe, RequestCode.Close);

injector.Eject(DllPath);
