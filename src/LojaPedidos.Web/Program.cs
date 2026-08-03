using LojaPedidos.Web;
using LojaPedidos.Web.Clients.Pedidos;
using LojaPedidos.Web.Clients.Produtos;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddMudServices();

var apiBaseUrl = builder.Configuration["Api:BaseUrl"]
    ?? throw new InvalidOperationException("O endereço da API não foi configurado.");

builder.Services.AddScoped(_ => new HttpClient
{
    BaseAddress = new Uri(apiBaseUrl)
});
builder.Services.AddScoped<IPedidosApiClient, PedidosApiClient>();
builder.Services.AddScoped<IProdutosApiClient, ProdutosApiClient>();

await builder.Build().RunAsync();
