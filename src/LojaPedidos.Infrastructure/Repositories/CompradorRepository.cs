using LojaPedidos.Domain.Entities;
using LojaPedidos.Domain.Repositories;

namespace LojaPedidos.Infrastructure.Repositories;

public sealed class CompradorRepository(LojaPedidosDbContext dbContext)
    : RepositoryBase<Comprador>(dbContext), ICompradorRepository;
