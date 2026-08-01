using LojaPedidos.Domain.Entities;
using LojaPedidos.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LojaPedidos.Infrastructure.Repositories;

public sealed class CompradorRepository(LojaPedidosDbContext dbContext)
    : RepositoryBase<Comprador>(dbContext), ICompradorRepository
{
    public Task<Comprador?> ObterPorCpfAsync(
        string cpf,
        CancellationToken cancellationToken = default)
    {
        return DbSet.SingleOrDefaultAsync(
            comprador => comprador.Cpf == cpf,
            cancellationToken);
    }
}
