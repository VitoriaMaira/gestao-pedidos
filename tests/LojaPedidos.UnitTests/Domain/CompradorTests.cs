using LojaPedidos.Domain.Entities;
using LojaPedidos.Domain.Exceptions;

namespace LojaPedidos.UnitTests.Domain;

public sealed class CompradorTests
{
    [Fact]
    public void Criar_DeveNormalizarCpfFormatado()
    {
        var comprador = new Comprador("João da Silva", "123.456.789-09");

        Assert.Equal("12345678909", comprador.Cpf);
    }

    [Fact]
    public void Criar_DeveFalhar_QuandoCpfForInvalido()
    {
        var acao = () => new Comprador("João da Silva", "11111111111");

        Assert.Throws<DomainException>(acao);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Criar_DeveFalhar_QuandoNomeForVazio(string nome)
    {
        var acao = () => new Comprador(nome, "12345678909");

        var excecao = Assert.Throws<DomainException>(acao);
        Assert.Equal("O nome do comprador é obrigatório.", excecao.Message);
    }
}
