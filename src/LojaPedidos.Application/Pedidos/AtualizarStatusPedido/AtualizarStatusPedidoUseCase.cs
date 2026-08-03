using FluentValidation;
using LojaPedidos.Application.Common.Exceptions;
using LojaPedidos.Application.Pedidos.ConsultarPedido;
using LojaPedidos.Domain.Enums;
using LojaPedidos.Domain.Repositories;

namespace LojaPedidos.Application.Pedidos.AtualizarStatusPedido;

public interface IAtualizarStatusPedidoUseCase
{
    Task<AtualizarStatusPedidoResponse> ExecutarAsync(
        Guid id,
        AtualizarStatusPedidoRequest request,
        CancellationToken cancellationToken = default);
}

public sealed class AtualizarStatusPedidoUseCase(
    IPedidoRepository pedidoRepository,
    IUnitOfWork unitOfWork,
    IValidator<AtualizarStatusPedidoRequest> validator)
    : IAtualizarStatusPedidoUseCase
{
    public async Task<AtualizarStatusPedidoResponse> ExecutarAsync(
        Guid id,
        AtualizarStatusPedidoRequest request,
        CancellationToken cancellationToken = default)
    {
        await validator.ValidateAndThrowAsync(request, cancellationToken);

        var pedido = await pedidoRepository.ObterPorId(id, cancellationToken);

        if (pedido is null)
        {
            throw new NotFoundException("Não foi possível encontrar o pedido informado.");
        }

        var statusJaDefinido = pedido.Status == request.Status;

        if (!statusJaDefinido && !TransicaoPermitida(pedido.Status, request.Status))
        {
            throw new ErrorOnValidationException(
                [$"Não é permitido alterar o status de {pedido.Status} para {request.Status}."]);
        }

        if (!statusJaDefinido)
        {
            pedido.DefinirStatus(request.Status);
        }

        await unitOfWork.CommitAsync(cancellationToken);

        var mensagem = statusJaDefinido
            ? $"O pedido já está com o status {request.Status}."
            : "Status do pedido atualizado com sucesso.";

        return new AtualizarStatusPedidoResponse(
            mensagem,
            ConsultarPedidoResponse.Map(pedido));
    }

    private static bool TransicaoPermitida(
        StatusPedido statusAtual,
        StatusPedido novoStatus)
    {
        return (statusAtual, novoStatus) switch
        {
            (StatusPedido.Iniciado, StatusPedido.Processado) => true,
            (StatusPedido.Iniciado, StatusPedido.Cancelado) => true,
            (StatusPedido.Processado, StatusPedido.Enviado) => true,
            (StatusPedido.Processado, StatusPedido.Cancelado) => true,
            _ => false
        };
    }
}
