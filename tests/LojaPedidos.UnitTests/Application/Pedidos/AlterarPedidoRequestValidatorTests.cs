using LojaPedidos.Application.Pedidos.AlterarPedido;

namespace LojaPedidos.UnitTests.Application.Pedidos;

public sealed class AlterarPedidoRequestValidatorTests
{
    private readonly AlterarPedidoRequestValidator _validator = new();

    [Fact]
    public async Task Validar_DeveSerValido_QuandoItensEstiveremCorretos()
    {
        var request = new AlterarPedidoRequest(
            [new AlterarItemPedidoRequest(Guid.CreateVersion7(), 2)]);

        var resultado = await _validator.ValidateAsync(request);

        Assert.True(resultado.IsValid);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task Validar_DeveFalhar_QuandoItemForInvalido(int quantidade)
    {
        var request = new AlterarPedidoRequest(
            [new AlterarItemPedidoRequest(Guid.Empty, quantidade)]);

        var resultado = await _validator.ValidateAsync(request);

        Assert.Contains(resultado.Errors, erro => erro.PropertyName == "Itens[0].ItemId");
        Assert.Contains(resultado.Errors, erro => erro.PropertyName == "Itens[0].Quantidade");
    }

    [Fact]
    public async Task Validar_DeveFalhar_QuandoItensEstiveremVazios()
    {
        var request = new AlterarPedidoRequest([]);

        var resultado = await _validator.ValidateAsync(request);

        Assert.Contains(
            resultado.Errors,
            erro => erro.PropertyName == nameof(AlterarPedidoRequest.Itens));
    }

    [Fact]
    public async Task Validar_DeveFalhar_QuandoItemForRepetido()
    {
        var itemId = Guid.CreateVersion7();
        var request = new AlterarPedidoRequest(
            [
                new AlterarItemPedidoRequest(itemId, 2),
                new AlterarItemPedidoRequest(itemId, 3)
            ]);

        var resultado = await _validator.ValidateAsync(request);

        Assert.Contains(
            resultado.Errors,
            erro => erro.PropertyName == nameof(AlterarPedidoRequest.Itens));
    }
}
