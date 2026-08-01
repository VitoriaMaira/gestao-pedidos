using LojaPedidos.Application.Pedidos.AlterarPedido;

namespace LojaPedidos.UnitTests.Application;

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

    [Fact]
    public async Task Validar_DeveFalhar_QuandoItemForInvalido()
    {
        var request = new AlterarPedidoRequest(
            [new AlterarItemPedidoRequest(Guid.Empty, 0)]);

        var resultado = await _validator.ValidateAsync(request);

        Assert.Contains(resultado.Errors, erro => erro.PropertyName == "Itens[0].ItemId");
        Assert.Contains(resultado.Errors, erro => erro.PropertyName == "Itens[0].Quantidade");
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
