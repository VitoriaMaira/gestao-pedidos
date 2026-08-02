namespace LojaPedidos.Application.Common.Exceptions;

public abstract class LojaPedidosException : SystemException
{
    protected LojaPedidosException(string message) : base(message) { }

    public abstract int StatusCode { get; }
    public abstract List<string> GetErrors();
}