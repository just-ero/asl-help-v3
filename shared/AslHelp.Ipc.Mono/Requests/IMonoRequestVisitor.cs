using AslHelp.Ipc.Mono.Responses;

namespace AslHelp.Ipc.Mono.Requests;

public interface IMonoRequestVisitor
{
    GetMonoImageResponse Handle(GetMonoImageRequest request);
}
