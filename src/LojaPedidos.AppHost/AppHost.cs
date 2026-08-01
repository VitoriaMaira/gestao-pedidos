var builder = DistributedApplication.CreateBuilder(args);

var sqlServer = builder.AddSqlServer("sqlserver")
    .WithDataVolume();
var database = sqlServer.AddDatabase("lojapedidos");

builder.AddProject<Projects.LojaPedidos_Api>("api")
    .WithReference(database)
    .WithEnvironment("ASPNETCORE_ENVIRONMENT", "Development")
    .WithEnvironment("DOTNET_ENVIRONMENT", "Development")
    .WithUrlForEndpoint("http", endpoint => new()
    {
        Url = $"{endpoint.Url}/swagger",
        DisplayText = "Swagger"
    })
    .WaitFor(database);

builder.Build().Run();
