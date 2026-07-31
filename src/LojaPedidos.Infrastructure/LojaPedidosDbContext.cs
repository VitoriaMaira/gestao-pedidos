using LojaPedidos.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LojaPedidos.Infrastructure;

public sealed class LojaPedidosDbContext(DbContextOptions<LojaPedidosDbContext> options)
    : DbContext(options)
{
    public DbSet<Comprador> Compradores => Set<Comprador>();

    public DbSet<Produto> Produtos => Set<Produto>();

    public DbSet<Pedido> Pedidos => Set<Pedido>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(LojaPedidosDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}
