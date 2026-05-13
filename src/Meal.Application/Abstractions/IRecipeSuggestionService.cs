using Meal.Core.Contracts;

namespace Meal.Application.Abstractions;

public interface IRecipeSuggestionService
{
    Task<RecipeSuggestionResponse> SuggestAsync(RecipeSuggestionRequest request, CancellationToken cancellationToken = default);
}
