using LojaPedidos.Application.Common.Exceptions;
using LojaPedidos.Domain.Enums;
using LojaPedidos.Domain.Repositories;

namespace LojaPedidos.Application.Pedidos.ExcluirPedido;

public interface IExcluirPedidoUseCase
{
    Task ExecutarAsync(Guid id, CancellationToken cancellationToken = default);
}

public sealed class ExcluirPedidoUseCase(
    IPedidoRepository pedidoRepository,
    IUnitOfWork unitOfWork) : IExcluirPedidoUseCase
{
    public async Task ExecutarAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var pedido = await pedidoRepository.ObterPorId(id, cancellationToken);

        if (pedido is null)
            throw new NotFoundException("Não foi possível cancelar o pedido porque ele não foi encontrado.");

        if (pedido.Status == StatusPedido.Enviado)
            throw new ErrorOnValidationException(["Um pedido enviado não pode ser cancelado."]);

        if (pedido.Status != StatusPedido.Cancelado)
            pedido.DefinirStatus(StatusPedido.Cancelado);

        await unitOfWork.CommitAsync(cancellationToken);

        return;
    }
}
