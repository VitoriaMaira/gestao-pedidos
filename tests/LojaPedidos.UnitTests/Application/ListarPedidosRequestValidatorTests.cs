using LojaPedidos.Application.Pedidos.ListarPedidos;

namespace LojaPedidos.UnitTests.Application;

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
}
