using LojaPedidos.Domain.Entities;
using LojaPedidos.Domain.Enums;
using LojaPedidos.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LojaPedidos.Infrastructure.Repositories;

public sealed class PedidoRepository(LojaPedidosDbContext dbContext)
    : RepositoryBase<Pedido>(dbContext), IPedidoRepository
{
    public override async Task<Pedido?> ObterPorIdAsync(
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
        Guid? compradorId = null,
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

        if (compradorId.HasValue)
        {
            consulta = consulta.Where(pedido => pedido.CompradorId == compradorId.Value);
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
