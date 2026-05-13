## 🚀 May The Fourth 2026 - Desafio Meal

Aplicação FullStack com IA que recebe ingredientes e tempo disponível (em minutos) e retorna uma receita sugerida pela IA.

## Stack utilizada

- .NET 10
- Microsoft Agent Framework 1.5.0 (GA)
- Blazor WebAssembly
- OpenAI

## Estrutura da solução

- `src/Meal.Api`: endpoint HTTP para sugestão de receita
- `src/Meal.Ai`: agente de receita (classe herdada de `AIAgent`)
- `src/Meal.Core`: contratos de entrada e saída
- `src/Meal.Application`: service layer e interfaces de abstração
- `src/Meal.Infra`: implementação OpenAI + factory + DI
- `src/Meal.Frontend`: interface Blazor WASM (tela principal)
- `tests/Meal.Application.Tests`: testes de serviço de aplicação
- `tests/Meal.Api.Tests`: testes do endpoint da API

## Fluxo da aplicação

1. O usuário informa os ingredientes e o tempo disponível.
2. O frontend chama `POST /api/recipes/suggest`.
3. A API usa o `RecipeSuggestionService`.
4. O service cria um agente via Factory Pattern.
5. O agente chama o modelo OpenAI e devolve a receita.
6. O frontend exibe o resultado abaixo dos inputs.

## Configuração

Defina a chave da OpenAI de uma das formas:

- Variável de ambiente `OPENAI_API_KEY`
- `src/Meal.Api/appsettings.json` em `OpenAI:ApiKey`

Também é possível ajustar o modelo em `OpenAI:Model` (padrão: `gpt-4o-mini`).

## Executando

### API

```bash
dotnet run --project .\src\Meal.Api\Meal.Api.csproj
```

### Frontend

```bash
dotnet run --project .\src\Meal.Frontend\Meal.Frontend.csproj
```

> O frontend usa `https://localhost:7111` como URL da API por padrão (`src/Meal.Frontend/wwwroot/appsettings.json`).

## Testes

```bash
dotnet test .\Meal.slnx
```
