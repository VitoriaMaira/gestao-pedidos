using LojaPedidos.Domain.Entities;

namespace LojaPedidos.Domain.Repositories;

public interface IProdutoRepository : IRepositoryBase<Produto>
{
    Task<Produto?> ObterPorNomeAsync(string nome);
}
