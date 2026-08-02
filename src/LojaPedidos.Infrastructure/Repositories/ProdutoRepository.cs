using LojaPedidos.Domain.Entities;
using LojaPedidos.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LojaPedidos.Infrastructure.Repositories;

public sealed class ProdutoRepository(LojaPedidosDbContext dbContext) : RepositoryBase<Produto>(dbContext), IProdutoRepository
{
    public Task<Produto?> ObterPorNomeAsync(string nome) => dbContext.Produtos.FirstOrDefaultAsync(p => p.Nome == nome);
}
