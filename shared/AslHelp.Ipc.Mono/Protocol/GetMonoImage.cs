using AslHelp.Ipc.Protocol;

namespace AslHelp.Ipc.Mono.Protocol;

public enum GetMonoImageExitCode : byte
{
    Ok,
    NotFound
}

public readonly record struct GetMonoImageRequest(
    string Name) : IRequest<GetMonoImageRequest, GetMonoImageResponse>;

public readonly record struct GetMonoImageResponse(
    ulong Address,
    string Name,
    string ModuleName,
    string FileName) : IResponse<GetMonoImageResponse, GetMonoImageRequest>;

public readonly struct GetMonoImageExitCodeDescriptor : IExitCodeDescriptor<GetMonoImageExitCode, GetMonoImageRequest>
{
    public GetMonoImageExitCode Ok => GetMonoImageExitCode.Ok;

    public bool IsOk(GetMonoImageExitCode exitCode)
    {
        return exitCode == Ok;
    }

    public string GetMessage(GetMonoImageExitCode exitCode, GetMonoImageRequest request)
    {
        return exitCode switch
        {
            GetMonoImageExitCode.Ok => $"Image '{request.Name}' loaded successfully.",
            GetMonoImageExitCode.NotFound => $"Image '{request.Name}' could not be found.",
            _ => $"Unknown exit code: {exitCode}."
        };
    }
}
