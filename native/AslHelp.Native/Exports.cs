using System.Diagnostics;
using System.IO.Pipes;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Threading.Tasks;

using AslHelp.Api;
using AslHelp.Api.Requests;
using AslHelp.Api.Responses;
using AslHelp.Native.Mono;

namespace AslHelp.Native;

internal static partial class Exports
{
    private static async Task HandleRequest(RequestCode code)
    {
        switch (code)
        {
            case RequestCode.GetMonoImage:
            {
                Trace.WriteLine("  => Waiting for Mono image request...");
                if (await Deserialize<MonoImageRequest>() is not { } request)
                {
                    Trace.WriteLine("     => Invalid request...");
                    await Serialize(ResponseCode.ErrInvalidRequest);
                    return;
                }

                Trace.WriteLine($"     => Received Mono image request: {request}");

                var response = new MonoImageResponse(
                    Address: MonoApi.mono_image_loaded(request.NameOrPath),
                    Name: request.NameOrPath,
                    ModuleName: $"{request.NameOrPath}.dll",
                    FileName: request.NameOrPath);

                Trace.WriteLine($"  => Sending Mono image response: {response}");

                await Serialize(ResponseCode.Success);
                await Serialize(response);

                Trace.WriteLine("     => Sent Mono image response.");

                break;
            }

            case RequestCode.None:
            {
                break;
            }

            default:
            {
                Trace.WriteLine("  => Invalid request...");
                await Serialize(ResponseCode.ErrInvalidRequest);
                break;
            }
        }
    }

    private static async Task Main()
    {
        Trace.WriteLine("  => Waiting for connection...");
        await _pipe.WaitForConnectionAsync();

        Trace.WriteLine("     => Connected!");

        while (true)
        {
            Trace.WriteLine("  => Waiting for request...");
            var code = await Deserialize<RequestCode>();

            Trace.WriteLine($"     => Received request: {code}");

            if (code == RequestCode.Close)
            {
                Trace.WriteLine("  => Closing connection...");
                break;
            }

            await HandleRequest(code);
        }
    }

    [UnmanagedCallersOnly(EntryPoint = ApiResourceStrings.ApiEntryPoint)]
    public static unsafe bool ApiEntryPoint()
    {
        Trace.WriteLine($"Entered {ApiResourceStrings.ApiEntryPoint}.");

        Task.Run(Main);

        Trace.WriteLine("Started main task.");

        return true;
    }

    private static readonly NamedPipeServerStream _pipe = new("asl-help");

    private static async Task<T?> Deserialize<T>()
    {
        return (T?)await JsonSerializer.DeserializeAsync(_pipe, typeof(T), ApiSerializerContext.Default);
    }

    private static Task Serialize<T>(T value)
    {
        return JsonSerializer.SerializeAsync(_pipe, value, typeof(T), ApiSerializerContext.Default);
    }
}
