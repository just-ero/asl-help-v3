using System;
using System.Diagnostics;
using System.IO.Pipes;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using AslHelp.Api;
using AslHelp.Api.Requests;
using AslHelp.Api.Responses;
using AslHelp.Native.Mono;
using AslHelp.Native.Mono.Metadata;

using static AslHelp.Api.ApiSerializer;

namespace AslHelp.Native;

internal static partial class Exports
{
    [UnmanagedCallersOnly(EntryPoint = ApiResourceStrings.ApiEntryPoint)]
    public static unsafe bool ApiEntryPoint()
    {
        Task.Run(Main);
        return true;
    }

    private static void Main()
    {
        using var pipe = new NamedPipeServerStream("asl-help");

        try
        {
            Trace.WriteLine("Waiting for connection...");
            pipe.WaitForConnection();

            while (true)
            {
                Trace.WriteLine("Waiting for request...");

                var code = Deserialize<RequestCode>(pipe);

                Trace.WriteLine($"  => Received request: {code}");

                if (code == RequestCode.Close)
                {
                    Trace.WriteLine("    => Closing connection...");
                    break;
                }

                HandleRequest(pipe, code);
            }
        }
        catch (Exception ex)
        {
            Trace.WriteLine($"  => Exception: {ex}");
        }
    }

    private static void HandleRequest(NamedPipeServerStream pipe, RequestCode code)
    {
        switch (code)
        {
            case RequestCode.GetMonoImage:
                GetMonoImage(pipe);
                break;
            case RequestCode.GetMonoClass:
                GetMonoClass(pipe);
                break;
            default:
                Serialize(pipe, ResponseCode.UnknownRequest);
                break;
        }
    }

    private static unsafe void GetMonoImage(NamedPipeServerStream pipe)
    {
        if (ReceivePacket<GetMonoImageRequest>(pipe) is { } request)
        {
            Trace.WriteLine($"    => Requested image: {request.NameOrPath}");

            var img = MonoApi.mono_image_loaded(request.NameOrPath);
            var response = new GetMonoImageResponse(
                Address: (ulong)img,
                Name: GetString(img->AssemblyName),
                ModuleName: GetString(img->ModuleName),
                FileName: GetString(img->Name));

            SendPacket(pipe, response);
        }
    }

    private static unsafe void GetMonoClass(NamedPipeServerStream pipe)
    {
        if (ReceivePacket<GetMonoClassRequest>(pipe) is { } request)
        {
            Trace.WriteLine($"  => Requested class: {request.Name}");

            var klass = MonoApi.mono_class_from_name_case((MonoImage*)request.Image, request.Namespace, request.Name);
            var response = new GetMonoClassResponse(
                Address: (ulong)klass);

            SendPacket(pipe, response);
        }
    }

    private static unsafe string GetString(sbyte* bytes)
    {
        var span = MemoryMarshal.CreateReadOnlySpanFromNullTerminated((byte*)bytes);
        return Encoding.UTF8.GetString(span);
    }
}
