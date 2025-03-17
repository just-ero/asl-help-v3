using System;
using System.Threading.Tasks;

using AslHelp.Shared.Results;

namespace AslHelp.Shared.Tasks;

public interface ICreationStage
{
    ICreationStage OnStart(Action action);

    ICatchStage Exec(Func<Result> func);
    ICatchStage Exec(Func<Task<Result>> func);
}

public interface ICreationStage<TResult>
{
    ICreationStage<TResult> WithStartupMessage(Action action);

    ICatchStage<TResult> Exec(Func<Result<TResult>> func);
    ICatchStage<TResult> Exec(Func<Task<Result<TResult>>> func);
}
