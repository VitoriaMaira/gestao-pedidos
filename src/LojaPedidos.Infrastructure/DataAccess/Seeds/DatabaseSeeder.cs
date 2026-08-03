using LojaPedidos.Domain.Entities;
using LojaPedidos.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace LojaPedidos.Infrastructure.DataAccess.Seeds;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(
        LojaPedidosDbContext dbContext,
        CancellationToken cancellationToken = default)
    {
        var produtos = await ObterOuCriarProdutosAsync(
            dbContext,
            cancellationToken);
        var compradores = await ObterOuCriarCompradoresAsync(
            dbContext,
            cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);

        var compradorIds = compradores
            .Select(comprador => comprador.Id)
            .ToArray();
        var possuiPedidosDeExemplo = await dbContext.Pedidos
            .AnyAsync(
                pedido => compradorIds.Contains(pedido.CompradorId),
                cancellationToken);

        if (possuiPedidosDeExemplo)
        {
            return;
        }

        var pedidos = CriarPedidos(produtos, compradores);
        await dbContext.Pedidos.AddRangeAsync(pedidos, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    private static async Task<IReadOnlyList<Produto>> ObterOuCriarProdutosAsync(
        LojaPedidosDbContext dbContext,
        CancellationToken cancellationToken)
    {
        var exemplos = new[]
        {
            new ProdutoSeed(
                Guid.Parse("01980000-0000-7000-8000-000000000001"),
                "Teclado mecânico",
                349.90m),
            new ProdutoSeed(
                Guid.Parse("01980000-0000-7000-8000-000000000002"),
                "Mouse sem fio",
                129.90m),
            new ProdutoSeed(
                Guid.Parse("01980000-0000-7000-8000-000000000003"),
                "Monitor 24 polegadas",
                899.90m),
            new ProdutoSeed(
                Guid.Parse("01980000-0000-7000-8000-000000000004"),
                "Webcam Full HD",
                249.90m),
            new ProdutoSeed(
                Guid.Parse("01980000-0000-7000-8000-000000000005"),
                "Headset USB",
                199.90m)
        };

        var ids = exemplos.Select(exemplo => exemplo.Id).ToArray();
        var existentes = await dbContext.Produtos
            .Where(produto => ids.Contains(produto.Id))
            .ToDictionaryAsync(produto => produto.Id, cancellationToken);

        var produtos = new List<Produto>();

        foreach (var exemplo in exemplos)
        {
            if (!existentes.TryGetValue(exemplo.Id, out var produto))
            {
                produto = new Produto
                {
                    Id = exemplo.Id,
                    Nome = exemplo.Nome,
                    Preco = exemplo.Preco
                };

                await dbContext.Produtos.AddAsync(produto, cancellationToken);
            }

            produtos.Add(produto);
        }

        return produtos;
    }

    private static async Task<IReadOnlyList<Comprador>> ObterOuCriarCompradoresAsync(
        LojaPedidosDbContext dbContext,
        CancellationToken cancellationToken)
    {
        var exemplos = new[]
        {
            new CompradorSeed(
                Guid.Parse("01980000-0000-7000-8000-000000000101"),
                "Ana Oliveira",
                "52998224725"),
            new CompradorSeed(
                Guid.Parse("01980000-0000-7000-8000-000000000102"),
                "Carlos Santos",
                "11144477735")
        };

        var cpfs = exemplos.Select(exemplo => exemplo.Cpf).ToArray();
        var existentes = await dbContext.Compradores
            .Where(comprador => cpfs.Contains(comprador.Cpf))
            .ToDictionaryAsync(comprador => comprador.Cpf, cancellationToken);

        var compradores = new List<Comprador>();

        foreach (var exemplo in exemplos)
        {
            if (!existentes.TryGetValue(exemplo.Cpf, out var comprador))
            {
                comprador = new Comprador(exemplo.Nome, exemplo.Cpf)
                {
                    Id = exemplo.Id
                };

                await dbContext.Compradores.AddAsync(comprador, cancellationToken);
            }

            compradores.Add(comprador);
        }

        return compradores;
    }

    private static IReadOnlyCollection<Pedido> CriarPedidos(
        IReadOnlyList<Produto> produtos,
        IReadOnlyList<Comprador> compradores)
    {
        var iniciado = new Pedido(
            compradores[0],
            [
                new ItemPedido(produtos[0], 1),
                new ItemPedido(produtos[1], 2)
            ]);

        var processado = new Pedido(
            compradores[1],
            [new ItemPedido(produtos[2], 1)]);
        processado.DefinirStatus(StatusPedido.Processado);

        var enviado = new Pedido(
            compradores[0],
            [
                new ItemPedido(produtos[3], 1),
                new ItemPedido(produtos[4], 1)
            ]);
        enviado.DefinirStatus(StatusPedido.Enviado);

        return [iniciado, processado, enviado];
    }

    private sealed record ProdutoSeed(Guid Id, string Nome, decimal Preco);

    private sealed record CompradorSeed(Guid Id, string Nome, string Cpf);
}
