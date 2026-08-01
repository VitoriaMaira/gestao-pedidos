using System.Net;
using System.Text;
using System.Text.Json;

namespace LojaPedidos.IntegrationTests.Pedidos;

public sealed class RespostaInvalidaTests
{
    [Fact]
    public async Task AtualizarStatus_DeveRetornarProblemDetails_QuandoJsonForInvalido()
    {
        using var client = new HttpClient
        {
            BaseAddress = new Uri(PedidosApiClient.ObterBaseUrl())
        };
        using var content = new StringContent(
            """{"status":"StatusInexistente"}""",
            Encoding.UTF8,
            "application/json");

        using var response = await client.PutAsync(
            $"/api/pedidos/{Guid.CreateVersion7()}/status",
            content);
        var json = await response.Content.ReadAsStringAsync();
        using var problem = JsonDocument.Parse(json);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);
        Assert.Equal(400, problem.RootElement.GetProperty("status").GetInt32());
        Assert.True(problem.RootElement.TryGetProperty("errors", out _));
    }
}
