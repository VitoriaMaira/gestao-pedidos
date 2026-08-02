using LojaPedidos.Domain.Entities;

namespace LojaPedidos.Domain.Repositories;

public interface IProdutoRepository : IRepositoryBase<Produto>
{
    Task<Produto?> ObterPorNomeAsync(string nome);

    Task<(IEnumerable<Produto> Itens, int Total)> ListarAsync(
        int pagina,
        int tamanhoPagina,
        CancellationToken cancellationToken = default);
}
