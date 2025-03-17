using System;
using System.Threading.Tasks;

namespace AslHelp.Shared.Tasks;

internal sealed class TaskBuilderAction
{
    private readonly DelegateType _type;

    private readonly Action? _sync;
    private readonly Func<Task>? _async;

    public TaskBuilderAction(Action callback)
    {
        _type = DelegateType.Sync;
        _sync = callback;
    }

    public TaskBuilderAction(Func<Task> callback)
    {
        _type = DelegateType.Async;
        _async = callback;
    }

    public async Task Invoke()
    {
        if (_type == DelegateType.Sync)
        {
            _sync!();
        }
        else
        {
            await _async!();
        }
    }
}

internal sealed class TaskBuilderFunc<TResult>
{
    private readonly DelegateType _type;

    private readonly Func<TResult>? _sync;
    private readonly Func<Task<TResult>>? _async;

    public TaskBuilderFunc(Func<TResult> callback)
    {
        _type = DelegateType.Sync;
        _sync = callback;
    }

    public TaskBuilderFunc(Func<Task<TResult>> callback)
    {
        _type = DelegateType.Async;
        _async = callback;
    }

    public async Task<TResult> Invoke()
    {
        if (_type == DelegateType.Sync)
        {
            return _sync!();
        }
        else
        {
            return await _async!();
        }
    }
}

internal enum DelegateType
{
    Sync,
    Async
}
