using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Meal.Frontend;
using Meal.Frontend.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<MealApiClient>(_ =>
{
    var apiBaseUrl = builder.Configuration["ApiBaseUrl"] ?? "https://localhost:7111";
    var httpClient = new HttpClient { BaseAddress = new Uri(apiBaseUrl) };
    return new MealApiClient(httpClient);
});

await builder.Build().RunAsync();
