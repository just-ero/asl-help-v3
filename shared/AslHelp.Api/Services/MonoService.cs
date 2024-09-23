using System.Runtime.InteropServices;
using System.Threading.Tasks;

using AslHelp.Api.Protobuf;

using Grpc.Core;

namespace AslHelp.Api.Services;

public sealed class MonoService : Mono.MonoBase
{
    public override Task<MonoImageResponse> GetImage(MonoImageRequest request, ServerCallContext context)
    {
        return Task.FromResult(new MonoImageResponse
        {
            Address = mono_image_loaded(request.FileOrPath)
        });

        [DllImport("mono.dll", EntryPoint = nameof(mono_image_loaded), CharSet = CharSet.Ansi)]
        static extern ulong mono_image_loaded(string path);
    }
}
