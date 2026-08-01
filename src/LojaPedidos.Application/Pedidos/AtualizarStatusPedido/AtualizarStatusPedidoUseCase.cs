using FluentValidation;
using LojaPedidos.Application.Pedidos.ConsultarPedido;
using LojaPedidos.Domain.Repositories;

namespace LojaPedidos.Application.Pedidos.AtualizarStatusPedido;

public sealed class AtualizarStatusPedidoUseCase(
    IPedidoRepository pedidoRepository,
    IUnitOfWork unitOfWork,
    IValidator<AtualizarStatusPedidoRequest> validator)
    : IAtualizarStatusPedidoUseCase
{
    public async Task<AtualizarStatusPedidoResponse?> ExecutarAsync(
        Guid id,
        AtualizarStatusPedidoRequest request,
        CancellationToken cancellationToken = default)
    {
        await validator.ValidateAndThrowAsync(request, cancellationToken);

        var pedido = await pedidoRepository.ObterPorIdAsync(id, cancellationToken);

        if (pedido is null)
        {
            return null;
        }

        var statusJaDefinido = pedido.Status == request.Status;

        pedido.AlterarStatus(request.Status);
        await unitOfWork.CommitAsync(cancellationToken);

        var mensagem = statusJaDefinido
            ? $"O pedido já está com o status {request.Status}."
            : "Status do pedido atualizado com sucesso.";

        return new AtualizarStatusPedidoResponse(
            mensagem,
            PedidoResponse.Criar(pedido));
    }
}
