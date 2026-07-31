var builder = DistributedApplication.CreateBuilder(args);

var sqlServer = builder.AddSqlServer("sqlserver")
    .WithDataVolume();
var database = sqlServer.AddDatabase("lojapedidos");

builder.AddProject<Projects.LojaPedidos_Api>("api")
    .WithReference(database)
    .WaitFor(database);

builder.Build().Run();
