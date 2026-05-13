using System.Net.Http.Json;
using Meal.Frontend.Models;

namespace Meal.Frontend.Services;

public sealed class MealApiClient(HttpClient httpClient)
{
    private readonly HttpClient _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));

    public async Task<RecipeSuggestionResponse> SuggestRecipeAsync(
        RecipeSuggestionRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var response = await _httpClient.PostAsJsonAsync("/api/recipes/suggest", request, cancellationToken);
        response.EnsureSuccessStatusCode();

        var payload = await response.Content.ReadFromJsonAsync<RecipeSuggestionResponse>(cancellationToken: cancellationToken);

        return payload ?? throw new InvalidOperationException("A API retornou um corpo de resposta vazio.");
    }
}
