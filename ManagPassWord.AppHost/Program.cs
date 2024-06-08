var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.PurchaseManagement_Market_API>("purchasemanagement-market-api");

builder.Build().Run();
