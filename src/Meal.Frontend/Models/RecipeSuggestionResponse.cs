namespace Meal.Frontend.Models;

public sealed class RecipeSuggestionResponse
{
    public required string RecipeText { get; init; }

    public required int AvailableMinutes { get; init; }
}
