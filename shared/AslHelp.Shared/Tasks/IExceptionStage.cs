using System;
using System.Threading.Tasks;

namespace AslHelp.Shared.Tasks;

public interface IExceptionStage<TException>
    where TException : Exception
{
    IExceptionStage<TException> When(Func<TException, bool> predicate);
    IExceptionStage<TException> OnFailure(Action<TException> action);

    ICatchStage Retry();
    ICatchStage Cancel();
    ICatchStage Throw();
}

public interface IExceptionStage<TResult, TException>
{
    IExceptionStage<TResult, TException> When(Func<TException, bool> predicate);
    IExceptionStage<TResult, TException> OnFailure(Action<TException> action);

    ICatchStage<TResult> Retry();
    ICatchStage<TResult> Cancel();
    ICatchStage<TResult> Throw();
}
