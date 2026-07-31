var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.LojaPedidos_Api>("api");

builder.Build().Run();
