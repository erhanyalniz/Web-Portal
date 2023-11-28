using BlazorWebApp;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using System;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Register HttpClient
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7283/") });

// Register AuthenticationService
builder.Services.AddScoped<AuthenticationService>();

// Register UserService with LocalStorage
builder.Services.AddScoped<UserService>();
// Registeration for local storage service
builder.Services.AddBlazoredLocalStorage(); 

await builder.Build().RunAsync();
