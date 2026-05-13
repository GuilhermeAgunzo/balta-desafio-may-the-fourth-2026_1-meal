var builder = DistributedApplication.CreateBuilder(args);

var api = builder.AddProject<Projects.Meal_Api>("meal-api");

builder.AddProject<Projects.Meal_Frontend>("meal-frontend")
    .WithReference(api)
    .WaitFor(api);

builder.Build().Run();
