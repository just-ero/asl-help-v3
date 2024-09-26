using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using AslHelp.Api;
using AslHelp.Api.Requests;
using AslHelp.Api.Responses;
using AslHelp.Api.Servers;
using AslHelp.Native.Mono;
using AslHelp.Native.Mono.Metadata;

[module: SkipLocalsInit]

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
        using var server = new MonoServer(ApiResourceStrings.PipeName)
        {
            GetMonoImageHandler = GetMonoImage,
            GetMonoClassHandler = GetMonoClass
        };

        try
        {
            Trace.WriteLine("Waiting for connection...");
            server.WaitForConnection();
            Trace.WriteLine("  => Connected!");

            while (true)
            {
                Trace.WriteLine("Waiting for request...");
                var code = server.ReceiveRequest();
                Trace.WriteLine($"  => Received request: {code}");

                if (code == RequestCode.Close)
                {
                    Trace.WriteLine("    => Closing connection...");
                    break;
                }

                server.ProcessRequest(code);
            }
        }
        catch (Exception ex)
        {
            Trace.WriteLine($"""
                Main loop threw an exception:
                {ex}
                """);
        }
    }

    private static unsafe GetMonoImageResponse GetMonoImage(GetMonoImageRequest request)
    {
        Trace.WriteLine($"    => Loading image: {request.NameOrPath}");

        var img = MonoApi.mono_image_loaded(request.NameOrPath);

        Trace.WriteLine($"      => Image loaded!");
        Trace.WriteLine($"        => Address: 0x{(ulong)img:X}");
        Trace.WriteLine($"        => File: {GetString(img->Name)}");

        return new(
            address: (ulong)img,
            name: GetString(img->AssemblyName),
            moduleName: GetString(img->ModuleName),
            fileName: GetString(img->Name));
    }

    private static unsafe GetMonoClassResponse GetMonoClass(GetMonoClassRequest request)
    {
        var fullName = string.IsNullOrEmpty(request.Namespace)
            ? request.Name
            : $"{request.Namespace}.{request.Name}";

        Trace.WriteLine($"    => Loading class: {fullName}");

        var klass = MonoApi.mono_class_from_name_case((MonoImage*)request.Image, request.Namespace, request.Name);

        Trace.WriteLine($"      => Class loaded!");
        Trace.WriteLine($"        => Address: 0x{(ulong)klass:X}");

        return new(
            address: (ulong)klass);
    }

    private static unsafe string GetString(sbyte* bytes)
    {
        var span = MemoryMarshal.CreateReadOnlySpanFromNullTerminated((byte*)bytes);
        return Encoding.UTF8.GetString(span);
    }
}
