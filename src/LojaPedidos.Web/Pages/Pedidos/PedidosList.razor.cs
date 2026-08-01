using System.Globalization;
using LojaPedidos.Web.Clients.Pedidos;
using LojaPedidos.Web.Contracts.Pedidos;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;

namespace LojaPedidos.Web.Pages.Pedidos;

public partial class PedidosList : IDisposable
{
    private static readonly CultureInfo CulturaBrasileira = CultureInfo.GetCultureInfo("pt-BR");

    private IReadOnlyCollection<PedidoResponse> _pedidos = [];
    private CancellationTokenSource? _carregamento;
    private string? _cpf;
    private StatusPedido? _status;
    private string? _mensagemErro;
    private int _pagina = 1;
    private int _tamanhoPagina = 10;
    private int _total;
    private bool _carregando;

    [Inject]
    private IPedidosApiClient PedidosApiClient { get; set; } = default!;

    private bool TemFiltros => !string.IsNullOrWhiteSpace(_cpf) || _status is not null;

    private int TotalDePaginas => Math.Max(1, (int)Math.Ceiling((double)_total / _tamanhoPagina));

    private string TextoDoResumo => _total == 1
        ? "1 pedido encontrado"
        : $"{_total} pedidos encontrados";

    protected override Task OnInitializedAsync() => CarregarPedidosAsync();

    private async Task CarregarPedidosAsync()
    {
        _carregamento?.Cancel();
        _carregamento?.Dispose();
        _carregamento = new CancellationTokenSource();
        var carregamentoAtual = _carregamento;

        _carregando = true;
        _mensagemErro = null;

        try
        {
            var query = new ListarPedidosQuery(
                _pagina,
                _tamanhoPagina,
                _status,
                _cpf);

            var resultado = await PedidosApiClient.ListarAsync(query, carregamentoAtual.Token);

            if (!resultado.Sucesso || resultado.Dados is null)
            {
                _pedidos = [];
                _total = 0;
                _mensagemErro = resultado.Mensagem ?? "Não foi possível consultar os pedidos.";
                return;
            }

            _pedidos = resultado.Dados.Itens;
            _pagina = resultado.Dados.Pagina;
            _tamanhoPagina = resultado.Dados.TamanhoPagina;
            _total = resultado.Dados.Total;
        }
        catch (OperationCanceledException)
        {
            // Uma nova consulta substituiu a anterior.
        }
        finally
        {
            if (ReferenceEquals(_carregamento, carregamentoAtual))
            {
                _carregando = false;
            }
        }
    }

    private async Task AplicarFiltrosAsync()
    {
        _pagina = 1;
        await CarregarPedidosAsync();
    }

    private async Task LimparFiltrosAsync()
    {
        _cpf = null;
        _status = null;
        _pagina = 1;
        await CarregarPedidosAsync();
    }

    private async Task AlterarPaginaAsync(int pagina)
    {
        _pagina = pagina;
        await CarregarPedidosAsync();
    }

    private async Task AlterarTamanhoPaginaAsync(int tamanhoPagina)
    {
        _tamanhoPagina = tamanhoPagina;
        _pagina = 1;
        await CarregarPedidosAsync();
    }

    private async Task TratarTeclaDoFiltroAsync(KeyboardEventArgs evento)
    {
        if (evento.Key == "Enter")
        {
            await AplicarFiltrosAsync();
        }
    }

    private static string ObterCodigo(Guid id) => id.ToString("N")[..8].ToUpperInvariant();

    private static string FormatarData(DateTimeOffset data) =>
        data.ToLocalTime().ToString("dd/MM/yyyy HH:mm", CulturaBrasileira);

    private static string FormatarValor(decimal valor) => valor.ToString("C", CulturaBrasileira);

    private static string FormatarQuantidadeDeItens(int quantidade) =>
        quantidade == 1 ? "1 item" : $"{quantidade} itens";

    private static Color ObterCorDoStatus(StatusPedido status) => status switch
    {
        StatusPedido.Iniciado => Color.Info,
        StatusPedido.Processado => Color.Warning,
        StatusPedido.Enviado => Color.Success,
        StatusPedido.Cancelado => Color.Error,
        _ => Color.Default
    };

    public void Dispose()
    {
        _carregamento?.Cancel();
        _carregamento?.Dispose();
    }
}

