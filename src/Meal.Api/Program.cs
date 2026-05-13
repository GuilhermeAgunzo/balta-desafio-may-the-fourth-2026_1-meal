using Meal.Application.Abstractions;
using Meal.Application.Services;
using Meal.Core.Contracts;
using Meal.Infra;

const string CorsPolicyName = "frontend";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddMealInfrastructure(builder.Configuration);
builder.Services.AddScoped<IRecipeSuggestionService, RecipeSuggestionService>();
builder.Services.AddCors(options =>
{
    options.AddPolicy(CorsPolicyName, policy =>
    {
        var origins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>();

        if (origins is { Length: > 0 })
        {
            policy.WithOrigins(origins).AllowAnyHeader().AllowAnyMethod();
            return;
        }

        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors(CorsPolicyName);

app.MapPost(
        "/api/recipes/suggest",
        async Task<IResult> (
            RecipeSuggestionRequest request,
            IRecipeSuggestionService recipeSuggestionService,
            CancellationToken cancellationToken) =>
        {
            if (request.AvailableMinutes <= 0)
            {
                return Results.BadRequest(new { error = "AvailableMinutes must be greater than zero." });
            }

            if (request.Ingredients.Count == 0)
            {
                return Results.BadRequest(new { error = "At least one ingredient is required." });
            }

            var result = await recipeSuggestionService.SuggestAsync(request, cancellationToken);
            return Results.Ok(result);
        })
    .WithName("SuggestRecipe")
    .WithSummary("Receives ingredients and available minutes and returns an AI recipe suggestion.");

app.Run();

public partial class Program;
