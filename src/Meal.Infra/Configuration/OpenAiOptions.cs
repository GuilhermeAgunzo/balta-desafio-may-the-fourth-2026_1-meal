namespace Meal.Infra.Configuration;

public sealed class OpenAiOptions
{
    public const string SectionName = "OpenAI";

    public string ApiKey { get; init; } = string.Empty;

    public string Model { get; init; } = "gpt-4o-mini";
}
