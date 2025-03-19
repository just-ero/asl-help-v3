using AslHelp.Shared.Results.Errors;

namespace AslHelp.Shared.Results;

public interface IResult
{
    bool IsOk { get; }
    bool IsErr { get; }

    IResultError? Error { get; }
}

public interface IResult<out TValue> : IResult
{
    TValue? Value { get; }
}
