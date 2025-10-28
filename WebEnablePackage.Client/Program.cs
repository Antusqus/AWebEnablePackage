using System;
using System.Net.Http;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

// Register HttpClient for the WebAssembly client so components can inject HttpClient
var apiBase = builder.Configuration.GetValue<string>("ApiBase");
var baseUri = !string.IsNullOrWhiteSpace(apiBase) ? new Uri(apiBase) : new Uri(builder.HostEnvironment.BaseAddress);
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = baseUri });

builder.Services.AddMudServices();

await builder.Build().RunAsync();
