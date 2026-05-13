using Meal.Ai.Agents;
using Meal.Application.Abstractions;
using Meal.Infra.Configuration;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Configuration;
using OpenAI.Chat;

namespace Meal.Infra.Agents;

public sealed class OpenAiRecipeSuggestionAgentFactory : IRecipeSuggestionAgentFactory
{
    private readonly IChatClient _chatClient;

    public OpenAiRecipeSuggestionAgentFactory(IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(configuration);

        var section = configuration.GetSection(OpenAiOptions.SectionName);
        var configuredOptions = new OpenAiOptions
        {
            ApiKey = section["ApiKey"] ?? string.Empty,
            Model = section["Model"] ?? "gpt-4o-mini"
        };

        var apiKey = ResolveApiKey(configuredOptions.ApiKey);

        if (string.IsNullOrWhiteSpace(apiKey))
        {
            throw new InvalidOperationException(
                "OpenAI ApiKey was not configured. Set OpenAI:ApiKey in appsettings or OPENAI_API_KEY environment variable.");
        }

        var model = string.IsNullOrWhiteSpace(configuredOptions.Model)
            ? "gpt-4o-mini"
            : configuredOptions.Model.Trim();

        _chatClient = new ChatClient(model, apiKey).AsIChatClient();
    }

    public IRecipeSuggestionAgent Create()
    {
        var aiAgent = new RecipeSuggestionAgent(_chatClient);
        return new OpenAiRecipeSuggestionAgent(aiAgent);
    }

    private static string ResolveApiKey(string configuredApiKey)
    {
        if (!string.IsNullOrWhiteSpace(configuredApiKey))
        {
            return configuredApiKey.Trim();
        }

        return Environment.GetEnvironmentVariable("OPENAI_API_KEY") ?? string.Empty;
    }
}
