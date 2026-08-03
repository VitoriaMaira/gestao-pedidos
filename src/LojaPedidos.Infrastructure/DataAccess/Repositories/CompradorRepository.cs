using LojaPedidos.Domain.Entities;
using LojaPedidos.Domain.Repositories;
using LojaPedidos.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace LojaPedidos.Infrastructure.DataAccess.Repositories;

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
