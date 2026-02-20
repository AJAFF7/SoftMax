using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using BlazorAuthApp;
using BlazorAuthApp.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Load API URL from configuration
var apiBaseUrl = builder.Configuration["ApiBaseUrl"] ?? "http://localhost:5230/api";
Console.WriteLine($"API Base URL: {apiBaseUrl}");

// Configure HttpClient for API calls with BaseAddress
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddSingleton(new ApiConfiguration { BaseUrl = apiBaseUrl });

builder.Services.AddScoped<LocalStorageService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<DoctorService>();
builder.Services.AddScoped<AppointmentService>();
builder.Services.AddScoped<AssistantService>();

var host = builder.Build();

// Initialize AuthService
var authService = host.Services.GetRequiredService<AuthService>();
await authService.InitializeAsync();

await host.RunAsync();
