using Meal.Application.Abstractions;
using Meal.Application.Services;
using Meal.Core.Contracts;

namespace Meal.Application.Tests;

public sealed class RecipeSuggestionServiceTests
{
    [Fact]
    public async Task SuggestAsync_ShouldBuildPromptWithIngredientsAndTime()
    {
        var fakeAgent = new FakeRecipeSuggestionAgent("Receita validada");
        var service = new RecipeSuggestionService(new FakeRecipeSuggestionAgentFactory(fakeAgent));

        var response = await service.SuggestAsync(new RecipeSuggestionRequest
        {
            Ingredients = ["Frango", "Arroz", "Alho"],
            AvailableMinutes = 30
        });

        Assert.Equal("Receita validada", response.RecipeText);
        Assert.Equal(30, response.AvailableMinutes);
        Assert.Contains("Frango", fakeAgent.LastPrompt);
        Assert.Contains("30", fakeAgent.LastPrompt);
    }

    [Fact]
    public async Task SuggestAsync_ShouldThrowWhenIngredientsAreMissing()
    {
        var fakeAgent = new FakeRecipeSuggestionAgent("Receita");
        var service = new RecipeSuggestionService(new FakeRecipeSuggestionAgentFactory(fakeAgent));

        await Assert.ThrowsAsync<ArgumentException>(() => service.SuggestAsync(new RecipeSuggestionRequest
        {
            Ingredients = [],
            AvailableMinutes = 20
        }));
    }

    private sealed class FakeRecipeSuggestionAgent(string response) : IRecipeSuggestionAgent
    {
        public string LastPrompt { get; private set; } = string.Empty;

        public Task<string> RunAsync(string prompt, CancellationToken cancellationToken = default)
        {
            LastPrompt = prompt;
            return Task.FromResult(response);
        }
    }

    private sealed class FakeRecipeSuggestionAgentFactory(FakeRecipeSuggestionAgent fakeRecipeSuggestionAgent) : IRecipeSuggestionAgentFactory
    {
        public IRecipeSuggestionAgent Create() => fakeRecipeSuggestionAgent;
    }
}
