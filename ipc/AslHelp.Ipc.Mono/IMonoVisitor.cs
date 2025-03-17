using AslHelp.Ipc.Mono.Commands;
using AslHelp.Shared.Results;

namespace AslHelp.Ipc.Mono;

public interface IMonoVisitor
{
    Result<GetMonoImageResponse> GetMonoImage(GetMonoImageRequest request);
    Result<GetMonoClassResponse> GetMonoClass(GetMonoClassRequest request);
}
