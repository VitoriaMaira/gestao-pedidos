using LojaPedidos.Domain.Entities;
using LojaPedidos.Domain.Repositories;

namespace LojaPedidos.Infrastructure.Repositories;

public sealed class ProdutoRepository(LojaPedidosDbContext dbContext)
    : RepositoryBase<Produto>(dbContext), IProdutoRepository;
