using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;

namespace Meal.Ai.Agents;

public sealed class RecipeSuggestionAgent(IChatClient chatClient)
    : DelegatingAIAgent(
        new ChatClientAgent(
            chatClient,
            instructions: SystemInstructions,
            name: "MealRecipeAgent",
            description: "Sugere receitas com base em ingredientes e tempo disponível."))
{
    private const string SystemInstructions =
        """
        Você é o agente de sugestão de receitas do projeto Meal.
        Sempre responda em português do Brasil.
        Gere uma receita objetiva, segura e compatível com os ingredientes e o tempo disponível.
        Evite respostas longas demais e não inclua conteúdos fora do contexto culinário.
        """;
}
