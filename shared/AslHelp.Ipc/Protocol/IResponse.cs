namespace AslHelp.Ipc.Protocol;

public interface IResponse<TSelf>
    where TSelf : IResponse<TSelf>;

public interface IResponse<TSelf, TRequest>
    where TSelf : IResponse<TSelf, TRequest>
    where TRequest : IRequest<TRequest, TSelf>;
