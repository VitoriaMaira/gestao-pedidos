using LojaPedidos.Application.Pedidos.ListarPedidos;

namespace LojaPedidos.UnitTests.Application.Pedidos;

public sealed class ListarPedidosRequestValidatorTests
{
    private readonly ListarPedidosRequestValidator _validator = new();

    [Fact]
    public async Task Validar_DeveSerValido_QuandoCpfEstiverFormatado()
    {
        var request = new ListarPedidosRequest(Cpf: "123.456.789-09");

        var resultado = await _validator.ValidateAsync(request);

        Assert.True(resultado.IsValid);
    }

    [Fact]
    public async Task Validar_DeveFalhar_QuandoCpfForInvalido()
    {
        var request = new ListarPedidosRequest(Cpf: "11111111111");

        var resultado = await _validator.ValidateAsync(request);

        Assert.Contains(
            resultado.Errors,
            erro => erro.PropertyName == nameof(ListarPedidosRequest.Cpf));
    }

    [Theory]
    [InlineData(0, 10, nameof(ListarPedidosRequest.Pagina))]
    [InlineData(1, 0, nameof(ListarPedidosRequest.TamanhoPagina))]
    [InlineData(1, 101, nameof(ListarPedidosRequest.TamanhoPagina))]
    public async Task Validar_DeveFalhar_QuandoPaginacaoForInvalida(
        int pagina,
        int tamanhoPagina,
        string propriedadeInvalida)
    {
        var request = new ListarPedidosRequest(pagina, tamanhoPagina);

        var resultado = await _validator.ValidateAsync(request);

        Assert.Contains(
            resultado.Errors,
            erro => erro.PropertyName == propriedadeInvalida);
    }
}
