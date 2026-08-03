using LojaPedidos.Domain.Entities;
using LojaPedidos.Domain.Enums;
using LojaPedidos.Domain.Repositories;
using LojaPedidos.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace LojaPedidos.Infrastructure.DataAccess.Repositories;

public sealed class PedidoRepository(LojaPedidosDbContext dbContext)
    : RepositoryBase<Pedido>(dbContext), IPedidoRepository
{
    public override async Task<Pedido?> ObterPorId(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Include(pedido => pedido.Comprador)
            .Include(pedido => pedido.Itens)
                .ThenInclude(item => item.Produto)
            .SingleOrDefaultAsync(pedido => pedido.Id == id, cancellationToken);
    }

    public async Task<(IReadOnlyCollection<Pedido> Itens, int Total)> ListarAsync(
        int pagina,
        int tamanhoPagina,
        StatusPedido? status = null,
        string? cpf = null,
        CancellationToken cancellationToken = default)
    {
        var consulta = DbSet
            .AsNoTracking()
            .Include(pedido => pedido.Comprador)
            .Include(pedido => pedido.Itens)
                .ThenInclude(item => item.Produto)
            .AsQueryable();

        if (status.HasValue)
        {
            consulta = consulta.Where(pedido => pedido.Status == status.Value);
        }

        if (!string.IsNullOrWhiteSpace(cpf))
        {
            consulta = consulta.Where(pedido => pedido.Comprador.Cpf == cpf);
        }

        var total = await consulta.CountAsync(cancellationToken);

        var pedidos = await consulta
            .OrderByDescending(pedido => pedido.CriadoEm)
            .Skip((pagina - 1) * tamanhoPagina)
            .Take(tamanhoPagina)
            .ToListAsync(cancellationToken);

        return (pedidos, total);
    }
}
