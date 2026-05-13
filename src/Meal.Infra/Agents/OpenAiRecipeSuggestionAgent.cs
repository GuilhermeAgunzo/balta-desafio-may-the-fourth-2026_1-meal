using Meal.Ai.Agents;
using Meal.Application.Abstractions;

namespace Meal.Infra.Agents;

public sealed class OpenAiRecipeSuggestionAgent(RecipeSuggestionAgent recipeSuggestionAgent) : IRecipeSuggestionAgent
{
    private readonly RecipeSuggestionAgent _recipeSuggestionAgent = recipeSuggestionAgent ?? throw new ArgumentNullException(nameof(recipeSuggestionAgent));

    public async Task<string> RunAsync(string prompt, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(prompt);

        var response = await _recipeSuggestionAgent.RunAsync(prompt, cancellationToken: cancellationToken);
        return response.Text;
    }
}
