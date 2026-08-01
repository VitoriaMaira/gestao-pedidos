using LojaPedidos.Application.Pedidos.AtualizarStatusPedido;
using LojaPedidos.Domain.Enums;

namespace LojaPedidos.UnitTests.Application;

public sealed class AtualizarStatusPedidoRequestValidatorTests
{
    private readonly AtualizarStatusPedidoRequestValidator _validator = new();

    [Theory]
    [InlineData(StatusPedido.Processado)]
    [InlineData(StatusPedido.Enviado)]
    [InlineData(StatusPedido.Cancelado)]
    public async Task Validar_DeveSerValido_QuandoStatusPuderSerAplicado(
        StatusPedido status)
    {
        var request = new AtualizarStatusPedidoRequest(status);

        var resultado = await _validator.ValidateAsync(request);

        Assert.True(resultado.IsValid);
    }

    [Fact]
    public async Task Validar_DeveFalhar_QuandoStatusForIniciado()
    {
        var request = new AtualizarStatusPedidoRequest(StatusPedido.Iniciado);

        var resultado = await _validator.ValidateAsync(request);

        Assert.Contains(
            resultado.Errors,
            erro => erro.PropertyName == nameof(AtualizarStatusPedidoRequest.Status));
    }

    [Fact]
    public async Task Validar_DeveFalhar_QuandoStatusNaoExistir()
    {
        var request = new AtualizarStatusPedidoRequest((StatusPedido)999);

        var resultado = await _validator.ValidateAsync(request);

        Assert.Contains(
            resultado.Errors,
            erro => erro.PropertyName == nameof(AtualizarStatusPedidoRequest.Status));
    }
}
