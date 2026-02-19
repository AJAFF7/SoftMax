using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using BlazorAuthApp;
using BlazorAuthApp.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Configure HttpClient for API calls - don't use BaseAddress so services can specify full URLs
builder.Services.AddScoped(sp => new HttpClient());

builder.Services.AddScoped<LocalStorageService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<DoctorService>();
builder.Services.AddScoped<AppointmentService>();

var host = builder.Build();

// Initialize AuthService
var authService = host.Services.GetRequiredService<AuthService>();
await authService.InitializeAsync();

await host.RunAsync();
