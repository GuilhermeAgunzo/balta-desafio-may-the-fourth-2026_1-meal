using System.Net;
using System.Net.Http.Json;
using Meal.Application.Abstractions;
using Meal.Core.Contracts;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Meal.Api.Tests;

public sealed class RecipeEndpointTests : IClassFixture<MealApiFactory>
{
    private readonly HttpClient _httpClient;

    public RecipeEndpointTests(MealApiFactory factory)
    {
        _httpClient = factory.CreateClient();
    }

    [Fact]
    public async Task SuggestRecipe_ShouldReturnOkWithRecipePayload()
    {
        var response = await _httpClient.PostAsJsonAsync(
            "/api/recipes/suggest",
            new RecipeSuggestionRequest
            {
                Ingredients = ["Tomate", "Macarrao"],
                AvailableMinutes = 15
            });

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var payload = await response.Content.ReadFromJsonAsync<RecipeSuggestionResponse>();
        Assert.NotNull(payload);
        Assert.Equal("Receita simulada para testes.", payload.RecipeText);
        Assert.Equal(15, payload.AvailableMinutes);
    }

    [Fact]
    public async Task SuggestRecipe_ShouldReturnBadRequestWhenMinutesIsInvalid()
    {
        var response = await _httpClient.PostAsJsonAsync(
            "/api/recipes/suggest",
            new RecipeSuggestionRequest
            {
                Ingredients = ["Tomate"],
                AvailableMinutes = 0
            });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}

public sealed class MealApiFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            services.RemoveAll<IRecipeSuggestionService>();
            services.AddScoped<IRecipeSuggestionService, StubRecipeSuggestionService>();
        });
    }

    private sealed class StubRecipeSuggestionService : IRecipeSuggestionService
    {
        public Task<RecipeSuggestionResponse> SuggestAsync(RecipeSuggestionRequest request, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(new RecipeSuggestionResponse
            {
                RecipeText = "Receita simulada para testes.",
                AvailableMinutes = request.AvailableMinutes
            });
        }
    }
}
