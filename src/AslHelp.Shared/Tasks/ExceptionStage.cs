using System;

namespace AslHelp.Shared.Tasks;

internal interface IExceptionStage
{
    Func<Exception, bool>? WhenCallback { get; }
    Action<Exception>? OnFailureCallback { get; }
}

internal sealed class ExceptionStage<TException>
    : IExceptionStage<TException>
    , IExceptionStage
    where TException : Exception
{
    private readonly ICatchStage _asCatchStage;

    public ExceptionStage(ICatchStage catchStage)
    {
        _asCatchStage = catchStage;
    }

    public FailureBehavior FailureBehavior { get; private set; }

    public Func<Exception, bool>? WhenCallback { get; private set; }
    public Action<Exception>? OnFailureCallback { get; private set; }

    IExceptionStage<TException> IExceptionStage<TException>.When(Func<TException, bool> predicate)
    {
        // WhenCallback = new(predicate);
        return this;
    }

    IExceptionStage<TException> IExceptionStage<TException>.OnFailure(Action<TException> action)
    {
        // OnFailureCallback = new(action);
        return this;
    }

    ICatchStage IExceptionStage<TException>.Retry()
    {
        FailureBehavior = FailureBehavior.Retry;
        return _asCatchStage;
    }

    ICatchStage IExceptionStage<TException>.Cancel()
    {
        FailureBehavior = FailureBehavior.Cancel;
        return _asCatchStage;
    }

    ICatchStage IExceptionStage<TException>.Throw()
    {
        FailureBehavior = FailureBehavior.Throw;
        return _asCatchStage;
    }
}

internal enum FailureBehavior
{
    Retry,
    Cancel,
    Throw
}
