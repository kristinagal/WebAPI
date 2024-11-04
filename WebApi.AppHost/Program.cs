var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.P099_DTO>("p099-dto");

builder.Build().Run();
