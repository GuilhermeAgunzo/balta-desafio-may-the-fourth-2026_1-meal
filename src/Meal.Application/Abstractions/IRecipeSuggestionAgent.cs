namespace Meal.Application.Abstractions;

public interface IRecipeSuggestionAgent
{
    Task<string> RunAsync(string prompt, CancellationToken cancellationToken = default);
}
