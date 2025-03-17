using System;

namespace AslHelp.Shared.Tasks;

public interface ICatchStage
{
    IExceptionStage<TException> Catch<TException>() where TException : Exception;
    ICatchStage WithRetries(uint retries);
    IFinalizeStage WithTimeout(uint msTimeout);
}

public interface ICatchStage<TResult>
{
    IExceptionStage<TResult, TException> Catch<TException>() where TException : Exception;
    ICatchStage<TResult> WithRetries(uint retries);
    IFinalizeStage<TResult> WithTimeout(uint msTimeout);
}
