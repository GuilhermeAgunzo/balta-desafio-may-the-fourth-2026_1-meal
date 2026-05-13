namespace Meal.Frontend.Models;

public sealed class RecipeSuggestionRequest
{
    public IReadOnlyCollection<string> Ingredients { get; init; } = Array.Empty<string>();

    public int AvailableMinutes { get; init; }
}
