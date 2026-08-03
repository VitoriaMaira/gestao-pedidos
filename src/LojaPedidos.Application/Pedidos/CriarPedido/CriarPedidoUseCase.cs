using FluentValidation;
using LojaPedidos.Application.Common.Exceptions;
using LojaPedidos.Domain.Entities;
using LojaPedidos.Domain.Repositories;
using LojaPedidos.Domain.ValueObjects;

namespace LojaPedidos.Application.Pedidos.CriarPedido;

public interface ICriarPedidoUseCase
{
    Task<CriarPedidoResponse> ExecutarAsync(
        CriarPedidoRequest request,
        CancellationToken cancellationToken = default);
}

public sealed class CriarPedidoUseCase(
    ICompradorRepository compradorRepository,
    IProdutoRepository produtoRepository,
    IPedidoRepository pedidoRepository,
    IUnitOfWork unitOfWork,
    IValidator<CriarPedidoRequest> validator) : ICriarPedidoUseCase
{
    public async Task<CriarPedidoResponse> ExecutarAsync(
        CriarPedidoRequest request,
        CancellationToken cancellationToken = default)
    {
        await validator.ValidateAndThrowAsync(request, cancellationToken);

        var comprador = await ObterOuCriarComprador(request, cancellationToken);

        var itens = await CriarItens(request.Itens, cancellationToken);

        var pedido = new Pedido(comprador, itens);

        await pedidoRepository.AdicionarAsync(pedido, cancellationToken);
        await unitOfWork.CommitAsync(cancellationToken);

        return new CriarPedidoResponse(pedido.Id);
    }

    private async Task<Comprador> ObterOuCriarComprador(CriarPedidoRequest request, CancellationToken cancellationToken)
    {
        var cpf = Cpf.Normalizar(request.CpfComprador);
        var comprador = await compradorRepository.ObterPorCpfAsync(
            cpf,
            cancellationToken);

        if (comprador is not null)
            return comprador;

        comprador = new Comprador(request.NomeComprador, cpf);
        await compradorRepository.AdicionarAsync(comprador, cancellationToken);

        return comprador;
    }

    private async Task<IReadOnlyCollection<ItemPedido>> CriarItens(IEnumerable<CriarPedidoRequest_ItemPedidoAux> itensRequest, CancellationToken cancellationToken)
    {
        var itens = new List<ItemPedido>();

        foreach (var itemRequest in itensRequest)
        {
            var produto = await produtoRepository.ObterPorId(itemRequest.Id, cancellationToken);

            if (produto is null)
                throw new NotFoundException($"Produto com o identificador '{itemRequest.Id}' não encontrado.");

            itens.Add(new ItemPedido(produto, itemRequest.Quantidade));
        }

        return itens;
    }
}
