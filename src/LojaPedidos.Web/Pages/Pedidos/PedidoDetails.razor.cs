using System.Globalization;
using System.Net;
using LojaPedidos.Web.Clients.Pedidos;
using LojaPedidos.Web.Contracts.Pedidos;
using LojaPedidos.Web.Formatting;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace LojaPedidos.Web.Pages.Pedidos;

public partial class PedidoDetails : IDisposable
{
    private static readonly CultureInfo CulturaBrasileira = CultureInfo.GetCultureInfo("pt-BR");
    private CancellationTokenSource? _carregamento;
    private PedidoResponse? _pedido;
    private string? _mensagemErro;
    private bool _carregando;
    private bool _naoEncontrado;
    private bool _editando;
    private bool _executando;
    private string? _mensagemOperacao;
    private readonly Dictionary<Guid, int> _quantidades = [];
    private IReadOnlyDictionary<string, string[]> _errosOperacao =
        new Dictionary<string, string[]>(StringComparer.OrdinalIgnoreCase);

    [Parameter]
    public Guid Id { get; set; }

    [Inject]
    private IPedidosApiClient PedidosApiClient { get; set; } = default!;

    [Inject]
    private IDialogService DialogService { get; set; } = default!;

    [Inject]
    private ISnackbar Snackbar { get; set; } = default!;

    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

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
            _editando = false;
            _quantidades.Clear();
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

    private void AtivarEdicao()
    {
        if (_pedido is null)
        {
            return;
        }

        _quantidades.Clear();
        foreach (var item in _pedido.Itens)
        {
            _quantidades[item.Id] = item.Quantidade;
        }

        _mensagemOperacao = null;
        _errosOperacao = new Dictionary<string, string[]>(StringComparer.OrdinalIgnoreCase);
        _editando = true;
    }

    private void CancelarEdicao()
    {
        _editando = false;
        _mensagemOperacao = null;
        _errosOperacao = new Dictionary<string, string[]>(StringComparer.OrdinalIgnoreCase);
        _quantidades.Clear();
    }

    private int ObterQuantidade(Guid itemId) => _quantidades.GetValueOrDefault(itemId, 1);

    private void DefinirQuantidade(Guid itemId, int quantidade) => _quantidades[itemId] = quantidade;

    private string? ObterErroQuantidade(Guid itemId)
    {
        if (_pedido is null)
        {
            return null;
        }

        var indice = _pedido.Itens.ToList().FindIndex(item => item.Id == itemId);
        var campo = $"Itens[{indice}].Quantidade";
        return _errosOperacao.TryGetValue(campo, out var mensagens)
            ? mensagens.FirstOrDefault()
            : null;
    }

    private async Task SalvarAlteracoesAsync()
    {
        if (_pedido is null || _quantidades.Values.Any(quantidade => quantidade <= 0))
        {
            _mensagemOperacao = "A quantidade deve ser maior que zero.";
            return;
        }

        var request = new AlterarPedidoRequest(
            _quantidades.Select(item => new AlterarItemPedidoRequest(item.Key, item.Value)).ToArray());
        var resultado = await ExecutarAsync(token => PedidosApiClient.AlterarAsync(Id, request, token));

        if (resultado is null)
        {
            return;
        }

        _pedido = resultado;
        _editando = false;
        Snackbar.Add("Pedido atualizado com sucesso.", Severity.Success);
    }

    private Task ProcessarAsync() => AtualizarStatusAsync(StatusPedido.Processado);

    private Task EnviarAsync() => AtualizarStatusAsync(StatusPedido.Enviado);

    private async Task CancelarAsync()
    {
        var confirmado = await DialogService.ShowMessageBoxAsync(
            "Cancelar pedido",
            "Deseja cancelar este pedido?",
            yesText: "Cancelar pedido",
            cancelText: "Voltar");

        if (confirmado == true)
        {
            await AtualizarStatusAsync(StatusPedido.Cancelado);
        }
    }

    private async Task AtualizarStatusAsync(StatusPedido status)
    {
        var resultado = await ExecutarAsync(token => PedidosApiClient.AtualizarStatusAsync(
            Id, new AtualizarStatusPedidoRequest(status), token));

        if (resultado is null)
        {
            return;
        }

        _pedido = resultado.Pedido;
        Snackbar.Add(resultado.Mensagem, Severity.Success);
    }

    private async Task ExcluirAsync()
    {
        var confirmado = await DialogService.ShowMessageBoxAsync(
            "Excluir pedido",
            "O pedido será cancelado e permanecerá no histórico. Deseja continuar?",
            yesText: "Excluir",
            cancelText: "Voltar");

        if (confirmado != true)
        {
            return;
        }

        var resultado = await ExecutarAsync(token => PedidosApiClient.ExcluirAsync(Id, token));
        if (resultado is null)
        {
            return;
        }

        Snackbar.Add(resultado.Mensagem, Severity.Success);
        NavigationManager.NavigateTo("/pedidos");
    }

    private async Task<T?> ExecutarAsync<T>(Func<CancellationToken, Task<LojaPedidos.Web.Contracts.Common.ApiResult<T>>> operacao)
    {
        _executando = true;
        _mensagemOperacao = null;
        _errosOperacao = new Dictionary<string, string[]>(StringComparer.OrdinalIgnoreCase);

        try
        {
            var resultado = await operacao(CancellationToken.None);
            if (!resultado.Sucesso || resultado.Dados is null)
            {
                _mensagemOperacao = resultado.Mensagem ?? "Não foi possível concluir a operação.";
                _errosOperacao = resultado.Erros is null
                    ? new Dictionary<string, string[]>(StringComparer.OrdinalIgnoreCase)
                    : new Dictionary<string, string[]>(resultado.Erros, StringComparer.OrdinalIgnoreCase);
                return default;
            }

            return resultado.Dados;
        }
        finally
        {
            _executando = false;
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
