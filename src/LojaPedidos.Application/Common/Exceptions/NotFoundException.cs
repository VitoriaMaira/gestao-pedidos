using System.Net;

namespace LojaPedidos.Application.Common.Exceptions;

public sealed class NotFoundException : LojaPedidosException
{
    public NotFoundException(string message) : base(message) { }

    public override int StatusCode => (int)HttpStatusCode.NotFound;

    public override List<string> GetErrors()
    {
        return [Message];
    }
}