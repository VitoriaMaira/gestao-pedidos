using System.Globalization;
using System.Net;
using LojaPedidos.Web.Clients.Pedidos;
using LojaPedidos.Web.Contracts.Pedidos;
using LojaPedidos.Web.Formatting;
using Microsoft.AspNetCore.Components;

namespace LojaPedidos.Web.Pages.Pedidos;

public partial class PedidoDetails : IDisposable
{
    private static readonly CultureInfo CulturaBrasileira = CultureInfo.GetCultureInfo("pt-BR");
    private CancellationTokenSource? _carregamento;
    private PedidoResponse? _pedido;
    private string? _mensagemErro;
    private bool _carregando;
    private bool _naoEncontrado;

    [Parameter]
    public Guid Id { get; set; }

    [Inject]
    private IPedidosApiClient PedidosApiClient { get; set; } = default!;

    protected override Task OnParametersSetAsync() => CarregarPedidoAsync();

    private async Task CarregarPedidoAsync()
    {
        _carregamento?.Cancel();
        _carregamento?.Dispose();
        _carregamento = new CancellationTokenSource();
        var carregamentoAtual = _carregamento;

        _carregando = true;
        _mensagemErro = null;
        _naoEncontrado = false;

        try
        {
            var resultado = await PedidosApiClient.ObterPorIdAsync(Id, carregamentoAtual.Token);

            if (!resultado.Sucesso || resultado.Dados is null)
            {
                _pedido = null;
                _naoEncontrado = resultado.StatusCode == HttpStatusCode.NotFound;
                _mensagemErro = resultado.Mensagem ?? "Não foi possível consultar o pedido.";
                return;
            }

            _pedido = resultado.Dados;
        }
        catch (OperationCanceledException)
        {
        }
        finally
        {
            if (ReferenceEquals(_carregamento, carregamentoAtual))
            {
                _carregando = false;
            }
        }
    }

    private static string FormatarData(DateTimeOffset data) =>
        data.ToLocalTime().ToString("dd/MM/yyyy 'às' HH:mm", CulturaBrasileira);

    private static string FormatarValor(decimal valor) => FormatadorBrasileiro.FormatarMoeda(valor);

    private static string FormatarQuantidade(int quantidade) =>
        quantidade == 1 ? "1 item" : $"{quantidade} itens";

    public void Dispose()
    {
        _carregamento?.Cancel();
        _carregamento?.Dispose();
    }
}
