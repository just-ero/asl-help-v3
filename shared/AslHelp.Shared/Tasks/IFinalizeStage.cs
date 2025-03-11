using System;
using System.Threading.Tasks;

namespace AslHelp.Shared.Tasks;

public interface IFinalizeStage
{
    IFinalizeStage OnComplete(Action action);

    Task RunAsync();
}

public interface IFinalizeStage<TResult>
{
    IFinalizeStage OnCompletion(Action<TResult> action);

    Task<TResult> RunAsync();
}
