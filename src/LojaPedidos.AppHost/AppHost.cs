var builder = DistributedApplication.CreateBuilder(args);

var sqlServer = builder.AddSqlServer("sqlserver", port: 7000)
     .WithLifetime(ContainerLifetime.Persistent);


var database = sqlServer.AddDatabase("lojapedidos");

var api = builder.AddProject<Projects.LojaPedidos_Api>("api")
    .WithReference(database)
    .WithEnvironment("ASPNETCORE_ENVIRONMENT", "Development")
    .WithEnvironment("DOTNET_ENVIRONMENT", "Development")
    .WithUrlForEndpoint("http", endpoint => new()
    {
        Url = $"{endpoint.Url}/swagger",
        DisplayText = "Swagger"
    })
    .WaitFor(database);

builder.AddProject<Projects.LojaPedidos_Web>("web")
    .WithEnvironment("ASPNETCORE_ENVIRONMENT", "Development")
    .WithEnvironment("DOTNET_ENVIRONMENT", "Development")
    .WithUrlForEndpoint("http", endpoint => new()
    {
        Url = endpoint.Url,
        DisplayText = "Loja Pedidos Web"
    })
    .WaitFor(api);

builder.Build().Run();
