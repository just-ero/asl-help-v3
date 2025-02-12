namespace AslHelp.Ipc.Protocol;

public interface IRequest<TSelf>
    where TSelf : IRequest<TSelf>;

public interface IRequest<TSelf, TResponse>
    where TSelf : IRequest<TSelf, TResponse>
    where TResponse : IResponse<TResponse, TSelf>;
