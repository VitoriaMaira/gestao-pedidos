using LojaPedidos.Domain.Entities;
using LojaPedidos.Domain.Repositories;
using LojaPedidos.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace LojaPedidos.Infrastructure.DataAccess.Repositories;

public sealed class ProdutoRepository(LojaPedidosDbContext dbContext)
    : RepositoryBase<Produto>(dbContext), IProdutoRepository
{
    public Task<Produto?> ObterPorNomeAsync(string nome) =>
        DbSet.FirstOrDefaultAsync(produto => produto.Nome == nome);

    public async Task<(IEnumerable<Produto> Itens, int Total)> ListarAsync(
        int pagina,
        int tamanhoPagina,
        CancellationToken cancellationToken = default)
    {
        var consulta = DbSet.AsNoTracking();
        var total = await consulta.CountAsync(cancellationToken);

        var produtos = await consulta
            .OrderBy(produto => produto.Nome)
            .Skip((pagina - 1) * tamanhoPagina)
            .Take(tamanhoPagina)
            .ToListAsync(cancellationToken);

        return (produtos, total);
    }
}
