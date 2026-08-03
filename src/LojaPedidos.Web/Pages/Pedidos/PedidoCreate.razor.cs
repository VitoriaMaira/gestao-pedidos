using System.Globalization;
using LojaPedidos.Web.Clients.Pedidos;
using LojaPedidos.Web.Clients.Produtos;
using LojaPedidos.Web.Contracts.Pedidos;
using LojaPedidos.Web.Contracts.Produtos;
using LojaPedidos.Web.Formatting;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace LojaPedidos.Web.Pages.Pedidos;

public partial class PedidoCreate
{
    private const string ImagemPadrao = "images/product-placeholder.svg";
    private readonly CompradorFormulario _comprador = new();
    private readonly List<ItemFormulario> _itens = [];
    private readonly PatternMask _mascaraCpf = new("000.000.000-00");
    private MudForm? _form;
    private IReadOnlyCollection<ProdutoResponse> _produtos = [];
    private string? _mensagemErro;
    private bool _formularioValido;
    private bool _salvando;
    private bool _carregandoProdutos;

    [Inject] private IPedidosApiClient PedidosApiClient { get; set; } = default!;
    [Inject] private IProdutosApiClient ProdutosApiClient { get; set; } = default!;
    [Inject] private NavigationManager NavigationManager { get; set; } = default!;
    [Inject] private ISnackbar Snackbar { get; set; } = default!;

    private decimal TotalEstimado => _itens.Sum(item => item.Produto.Preco * item.Quantidade);
    private string TextoQuantidadeItens => _itens.Count == 1
        ? "1 produto selecionado"
        : $"{_itens.Count} produtos selecionados";

    protected override async Task OnInitializedAsync()
    {
        _carregandoProdutos = true;
        var resultado = await ProdutosApiClient.ListarAsync(new ListarProdutosQuery());
        _carregandoProdutos = false;

        if (!resultado.Sucesso || resultado.Dados is null)
        {
            _mensagemErro = resultado.Mensagem ?? "Não foi possível carregar os produtos.";
            return;
        }

        _produtos = resultado.Dados.Itens;
    }

    private bool EstaSelecionado(Guid produtoId) =>
        _itens.Any(item => item.Produto.Id == produtoId);

    private void AlternarProduto(ProdutoResponse produto)
    {
        var existente = _itens.SingleOrDefault(item => item.Produto.Id == produto.Id);
        if (existente is null)
            _itens.Add(new ItemFormulario(produto));
        else
            _itens.Remove(existente);
    }

    private static void DefinirQuantidade(ItemFormulario item, int quantidade) =>
        item.Quantidade = quantidade;

    private async Task CriarPedidoAsync()
    {
        if (_form is null || _salvando)
            return;

        await _form.ValidateAsync();
        if (!_formularioValido || _itens.Count == 0 || _itens.Any(item => item.Quantidade <= 0))
        {
            if (!_formularioValido)
            {
                Snackbar.Add(
                    "Quase lá! Confira os campos destacados e preencha as informações obrigatórias para continuar.",
                    Severity.Warning);
            }

            _mensagemErro = _itens.Count == 0
                ? "Selecione pelo menos um produto."
                : "Revise os campos destacados antes de criar o pedido.";
            return;
        }

        _salvando = true;
        _mensagemErro = null;

        var request = new CriarPedidoRequest(
            _comprador.Nome.Trim(),
            SomenteNumeros(_comprador.Cpf),
            _itens.Select(item => new CriarItemPedidoRequest(
                item.Produto.Id,
                item.Quantidade)).ToArray());
        var resultado = await PedidosApiClient.CriarAsync(request);
        _salvando = false;

        if (!resultado.Sucesso || resultado.Dados is null)
        {
            _mensagemErro = resultado.Mensagem ?? "Não foi possível criar o pedido.";
            return;
        }

        Snackbar.Add(resultado.Mensagem ?? "Pedido criado com sucesso.", Severity.Success);
        NavigationManager.NavigateTo($"/pedidos/{resultado.Dados.Id}");
    }

    private static string ObterImagem(string? imagemUrl) =>
        string.IsNullOrWhiteSpace(imagemUrl) ? ImagemPadrao : imagemUrl;

    private static string? ValidarCpf(string? valor)
    {
        var cpf = SomenteNumeros(valor);
        if (cpf.Length != 11 || cpf.Distinct().Count() == 1)
            return "Informe um CPF válido com 11 números.";

        var numeros = cpf.Select(caractere => caractere - '0').ToArray();
        return numeros[9] == CalcularDigito(numeros, 9, 10)
            && numeros[10] == CalcularDigito(numeros, 10, 11)
                ? null
                : "Informe um CPF válido.";
    }

    private static int CalcularDigito(int[] numeros, int quantidade, int pesoInicial)
    {
        var soma = Enumerable.Range(0, quantidade)
            .Sum(indice => numeros[indice] * (pesoInicial - indice));
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

    private sealed class ItemFormulario(ProdutoResponse produto)
    {
        public ProdutoResponse Produto { get; } = produto;
        public int Quantidade { get; set; } = 1;
    }
}
