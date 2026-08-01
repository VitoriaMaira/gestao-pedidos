using System.Globalization;
using LojaPedidos.Web.Clients.Pedidos;
using LojaPedidos.Web.Contracts.Pedidos;
using LojaPedidos.Web.Formatting;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace LojaPedidos.Web.Pages.Pedidos;

public partial class PedidoCreate
{
    internal static readonly CultureInfo CulturaBrasileira = CultureInfo.GetCultureInfo("pt-BR");

    private readonly CompradorFormulario _comprador = new();
    private readonly List<ItemFormulario> _itens = [new()];
    private readonly PatternMask _mascaraCpf = new("000.000.000-00");
    private MudForm? _form;
    private IReadOnlyCollection<string> _errosApi = [];
    private string? _mensagemErro;
    private bool _formularioValido;
    private bool _salvando;

    [Inject]
    private IPedidosApiClient PedidosApiClient { get; set; } = default!;

    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

    [Inject]
    private ISnackbar Snackbar { get; set; } = default!;

    private decimal TotalEstimado => _itens.Sum(item =>
        item.Preco.GetValueOrDefault() * item.Quantidade.GetValueOrDefault());

    private string TextoQuantidadeItens => _itens.Count == 1
        ? "1 item informado"
        : $"{_itens.Count} itens informados";

    private void AdicionarItem() => _itens.Add(new ItemFormulario());

    private void RemoverItem(ItemFormulario item)
    {
        if (_itens.Count > 1)
        {
            _itens.Remove(item);
        }
    }

    private async Task CriarPedidoAsync()
    {
        if (_form is null || _salvando)
        {
            return;
        }

        await _form.ValidateAsync();

        if (!_formularioValido)
        {
            _mensagemErro = "Revise os campos destacados antes de criar o pedido.";
            return;
        }

        _salvando = true;
        _mensagemErro = null;
        _errosApi = [];

        var request = new CriarPedidoRequest(
            new CriarCompradorRequest(
                _comprador.Nome.Trim(),
                SomenteNumeros(_comprador.Cpf)),
            _itens.Select(item => new CriarItemPedidoRequest(
                new CriarProdutoRequest(
                    item.NomeProduto.Trim(),
                    item.Preco!.Value),
                item.Quantidade!.Value))
            .ToArray());

        var resultado = await PedidosApiClient.CriarAsync(request);

        if (!resultado.Sucesso || resultado.Dados is null)
        {
            _mensagemErro = resultado.Mensagem ?? "Não foi possível criar o pedido.";
            _errosApi = resultado.Erros?
                .SelectMany(erro => erro.Value)
                .Distinct()
                .ToArray() ?? [];
            _salvando = false;
            return;
        }

        Snackbar.Add(resultado.Dados.Mensagem, Severity.Success);
        NavigationManager.NavigateTo($"/pedidos/{resultado.Dados.Id}");
    }

    private static string? ValidarCpf(string? valor)
    {
        var cpf = SomenteNumeros(valor);

        if (cpf.Length != 11 || cpf.Distinct().Count() == 1)
        {
            return "Informe um CPF válido com 11 números.";
        }

        var numeros = cpf.Select(caractere => caractere - '0').ToArray();
        var primeiroDigito = CalcularDigito(numeros, 9, 10);
        var segundoDigito = CalcularDigito(numeros, 10, 11);

        return numeros[9] == primeiroDigito && numeros[10] == segundoDigito
            ? null
            : "Informe um CPF válido.";
    }

    private static int CalcularDigito(int[] numeros, int quantidade, int pesoInicial)
    {
        var soma = 0;

        for (var indice = 0; indice < quantidade; indice++)
        {
            soma += numeros[indice] * (pesoInicial - indice);
        }

        var resto = soma % 11;
        return resto < 2 ? 0 : 11 - resto;
    }

    private static string SomenteNumeros(string? valor) =>
        new((valor ?? string.Empty).Where(char.IsDigit).ToArray());

    private static string FormatarValor(decimal valor) => FormatadorBrasileiro.FormatarMoeda(valor);

    private sealed class CompradorFormulario
    {
        public string Nome { get; set; } = string.Empty;
        public string Cpf { get; set; } = string.Empty;
    }

    private sealed class ItemFormulario
    {
        public Guid Id { get; } = Guid.NewGuid();
        public string NomeProduto { get; set; } = string.Empty;
        public decimal? Preco { get; set; }
        public int? Quantidade { get; set; } = 1;
    }
}
