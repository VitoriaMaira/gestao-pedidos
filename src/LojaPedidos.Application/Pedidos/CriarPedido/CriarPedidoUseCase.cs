using FluentValidation;
using LojaPedidos.Domain.Entities;
using LojaPedidos.Domain.Exceptions;
using LojaPedidos.Domain.Repositories;
using LojaPedidos.Domain.ValueObjects;

namespace LojaPedidos.Application.Pedidos.CriarPedido;

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

        var cpf = Cpf.Normalizar(request.Comprador!.Cpf);
        var comprador = await compradorRepository.ObterPorCpfAsync(cpf, cancellationToken);

        if (comprador is null)
        {
            comprador = new Comprador(request.Comprador.Nome!, cpf);
            await compradorRepository.AdicionarAsync(comprador, cancellationToken);
        }

        var itens = new List<ItemPedido>();

        foreach (var itemRequest in request.Itens!)
        {
            var produto = new Produto(
                itemRequest.Produto!.Nome!,
                itemRequest.Produto.Preco);

            await produtoRepository.AdicionarAsync(produto, cancellationToken);
            itens.Add(new ItemPedido(produto, itemRequest.Quantidade));
        }

        var pedido = new Pedido(comprador, itens);

        await pedidoRepository.AdicionarAsync(pedido, cancellationToken);
        await unitOfWork.CommitAsync(cancellationToken);

        return CriarPedidoResponse.Criar(pedido);
    }
}
