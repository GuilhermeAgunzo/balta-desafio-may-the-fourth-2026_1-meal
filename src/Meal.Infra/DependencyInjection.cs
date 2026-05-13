using Meal.Application.Abstractions;
using Meal.Infra.Agents;
using Meal.Infra.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Meal.Infra;

public static class DependencyInjection
{
    public static IServiceCollection AddMealInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IRecipeSuggestionAgentFactory, OpenAiRecipeSuggestionAgentFactory>();
        return services;
    }
}
