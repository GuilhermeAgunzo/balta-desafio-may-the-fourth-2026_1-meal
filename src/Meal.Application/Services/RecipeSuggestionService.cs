using Meal.Application.Abstractions;
using Meal.Core.Contracts;

namespace Meal.Application.Services;

public sealed class RecipeSuggestionService(IRecipeSuggestionAgentFactory recipeSuggestionAgentFactory) : IRecipeSuggestionService
{
    private readonly IRecipeSuggestionAgentFactory _recipeSuggestionAgentFactory = recipeSuggestionAgentFactory ?? throw new ArgumentNullException(nameof(recipeSuggestionAgentFactory));

    public async Task<RecipeSuggestionResponse> SuggestAsync(
        RecipeSuggestionRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (request.AvailableMinutes <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(request.AvailableMinutes), "AvailableMinutes must be greater than zero.");
        }

        var ingredients = NormalizeIngredients(request.Ingredients);

        if (ingredients.Count == 0)
        {
            throw new ArgumentException("At least one ingredient must be informed.", nameof(request.Ingredients));
        }

        var prompt = BuildPrompt(ingredients, request.AvailableMinutes);
        var agent = _recipeSuggestionAgentFactory.Create();
        var recipeText = await agent.RunAsync(prompt, cancellationToken);

        if (string.IsNullOrWhiteSpace(recipeText))
        {
            throw new InvalidOperationException("The AI agent returned an empty recipe suggestion.");
        }

        return new RecipeSuggestionResponse
        {
            RecipeText = recipeText.Trim(),
            AvailableMinutes = request.AvailableMinutes
        };
    }

    private static List<string> NormalizeIngredients(IEnumerable<string>? ingredients)
    {
        if (ingredients is null)
        {
            return [];
        }

        return ingredients
            .Select(value => value.Trim())
            .Where(value => !string.IsNullOrWhiteSpace(value))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();
    }

    private static string BuildPrompt(IReadOnlyCollection<string> ingredients, int availableMinutes)
    {
        var ingredientList = string.Join(", ", ingredients);

        return $$"""
            Você é um assistente culinário.
            Sua tarefa é sugerir UMA receita que possa ser feita com os ingredientes abaixo,
            respeitando o limite de tempo informado.

            Ingredientes disponíveis: {{ingredientList}}
            Tempo máximo disponível: {{availableMinutes}} minutos

            Regras:
            - Responda em português do Brasil.
            - Priorize usar apenas os ingredientes informados.
            - Se faltar algo essencial, cite substituições rápidas.
            - Entregue o resultado em texto puro com as seções:
              Nome da receita
              Tempo estimado
              Ingredientes
              Modo de preparo (passo a passo curto)
            """;
    }
}
