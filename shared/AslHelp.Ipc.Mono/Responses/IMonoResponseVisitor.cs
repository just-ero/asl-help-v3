using System.Threading.Tasks;

namespace AslHelp.Ipc.Mono.Responses;

public interface IMonoResponseVisitor
{
    Task Accept(GetMonoImageResponse response);
}
