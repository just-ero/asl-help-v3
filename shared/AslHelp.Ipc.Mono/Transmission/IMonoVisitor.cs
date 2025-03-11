using AslHelp.Ipc.Mono.Transmission.Commands;
using AslHelp.Shared.Results;

namespace AslHelp.Ipc.Mono.Transmission;

public interface IMonoVisitor
{
    Result<GetMonoImageResponse> GetMonoImage(GetMonoImageRequest request);
}
