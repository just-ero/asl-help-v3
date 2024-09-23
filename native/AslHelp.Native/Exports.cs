using System.Runtime.InteropServices;
using System.Threading.Tasks;

using AslHelp.Api.Services;
using AslHelp.Shared;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace AslHelp.Native;

internal static partial class Exports
{
    [UnmanagedCallersOnly(EntryPoint = NativeApi.StartListener)]
    public static unsafe bool StartListener()
    {
        var builder = WebApplication.CreateSlimBuilder();
        builder.Services.AddGrpc();

        var app = builder.Build();
        app.MapGrpcService<MonoService>();

        app.Run();

        return true;
    }
}
