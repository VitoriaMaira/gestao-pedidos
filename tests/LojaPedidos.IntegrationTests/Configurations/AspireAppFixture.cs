using Aspire.Hosting;
using Aspire.Hosting.Testing;
using LojaPedidos.IntegrationTests.Clients;

namespace LojaPedidos.IntegrationTests.Configurations;

public sealed class AspireAppFixture : IAsyncLifetime
{
    private static readonly TimeSpan Timeout = TimeSpan.FromMinutes(2);
    private DistributedApplication? _app;

    private HttpClient? _httpClient;

    public ApiClients Api { get; private set; } = null!;

    public async Task InitializeAsync()
    {
        var builder = await DistributedApplicationTestingBuilder
            .CreateAsync<Projects.LojaPedidos_AppHost>();

        _app = await builder.BuildAsync().WaitAsync(Timeout);
        await _app.StartAsync().WaitAsync(Timeout);
        await _app.ResourceNotifications
            .WaitForResourceHealthyAsync("api")
            .WaitAsync(Timeout);

        _httpClient = _app.CreateHttpClient("api");
        Api = ApiClients.Criar(_httpClient);
    }

    public async Task DisposeAsync()
    {
        _httpClient?.Dispose();
        if (_app is not null)
            await _app.DisposeAsync();
    }
}
