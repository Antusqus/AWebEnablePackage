using System;
using System.Collections.Generic;
using System.Linq;
using MudBlazor.Services;
using System.Net.Http;
using DataLibrary;
using WebEnablePackage.Client.Pages;
using WebEnablePackage.Components;
using System.Text.Json.Serialization;


var builder = WebApplication.CreateBuilder(args);

// Add MudBlazor services
builder.Services.AddMudServices();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddScoped<IDataAccess, DataAccess>();
// Register IHttpClientFactory and a scoped HttpClient named "api" so components can inject HttpClient
builder.Services.AddHttpClient("api", client =>
{
    // Default to the HTTP launch port from launchSettings.json (5142) for local dev.
    client.BaseAddress = new Uri(builder.Configuration.GetValue<string>("ApiBase") ?? "http://localhost:5142");
});

builder.Services.AddScoped(serviceProvider => serviceProvider.GetRequiredService<IHttpClientFactory>().CreateClient("api"));
// builder.Services.ConfigureHttpJsonOptions(opts =>
// {
//     opts.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
// });



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(WebEnablePackage.Client._Imports).Assembly);


// Add minimal API endpoints for the client to call
app.MapGet("/api/users", async (HttpContext context, DataLibrary.IDataAccess data, IConfiguration config) =>
{
    // Allow clients to request a comma-separated list of columns via ?columns=FirstName,LastName
    // Validate requested columns against a whitelist to prevent SQL injection.
    var conn = config.GetConnectionString("MySQLConnection") ?? throw new InvalidOperationException("Connection string 'MySQLConnection' is not configured.");

    var allowed = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        "Gender",
        "FirstName",
        "LastName",
        "UserName",
        "Email",
        "TeleNr",
        "Place",
        "FieldOfWork",
        "Education",
        "Experience",
        "DesiredFunction",
        "Motivation",
        "Location",
        "Comments"
    };

    var colsQuery = context.Request.Query["columns"].ToString();
    List<string> selectedCols;
    if (string.IsNullOrWhiteSpace(colsQuery))
    {
        selectedCols = new List<string> { "FirstName", "LastName" };
    }
    else
    {
        selectedCols = colsQuery.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Where(c => allowed.Contains(c))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();

        if (selectedCols.Count == 0)
        {
            // If the client requested columns but none are valid, return bad request with guidance.
            return Results.BadRequest(new { error = "No valid columns requested. Allowed: FirstName,LastName,UserName,Email" });
        }
    }

    // Quote column names with backticks for MySQL safety (allowed list ensures values are safe).
    var selectClause = string.Join(", ", selectedCols.Select(c => $"`{c}`"));
    string sql = $"SELECT {selectClause} FROM Users";

    // Use dynamic so callers can request different projections.
    var returnData = await data.LoadData<dynamic, dynamic>(sql, new { }, conn);
    return Results.Ok((List<dynamic>)returnData);
});

app.MapPost("/api/users", async (WebEnablePackage.ViewModels.VM_Registration model, DataLibrary.IDataAccess da, IConfiguration cfg) =>
{
    var conn = cfg.GetConnectionString("MySQLConnection") ?? throw new InvalidOperationException("Connection string 'MySQLConnection' is not configured.");
    string sql = "INSERT INTO Users (FirstName, LastName, UserName, Email) VALUES (@FirstName, @LastName, @UserName, @Email)";
    await da.SaveData(sql, new { model.FirstName, model.LastName }, conn);
    return Results.Created("/api/users", null);
});

// Typed endpoint for JobAgency views: returns VM_JobAgency projection (Gender, Experience etc.)
app.MapGet("/api/users/jobagency", async (DataLibrary.IDataAccess da, IConfiguration cfg) =>
{
    var conn = cfg.GetConnectionString("MySQLConnection") ?? throw new InvalidOperationException("Connection string 'MySQLConnection' is not configured.");
    // select the columns that map to VM_JobAgency; Dapper will map numeric enum columns to enum properties
    string sql = "SELECT Id, FirstName, LastName, Gender, TeleNr, Place, FieldOfWork, Education, Experience, DesiredFunction, Motivation, Location, Comments FROM Users";
    var data = await da.LoadData<WebEnablePackage.ViewModels.VM_JobAgency, dynamic>(sql, new { }, conn);
    return Results.Ok(data);
});

app.MapPost("/api/users/jobagency", async (WebEnablePackage.ViewModels.VM_JobAgency model, DataLibrary.IDataAccess da, IConfiguration cfg) =>
{
    var conn = cfg.GetConnectionString("MySQLConnection") ?? throw new InvalidOperationException("Connection string 'MySQLConnection' is not configured.");
    string sql = "INSERT INTO Users " +
    "(FirstName, LastName, Gender, TeleNr, Place, FieldOfWork, Education, Experience, DesiredFunction, Motivation, Location, Comments) " +
    "VALUES " +
    "(@FirstName, @LastName, @Gender, @TeleNr, @Place, @FieldOfWork, @Education, @Experience, @DesiredFunction, @Motivation, @Location, @Comments)";
    await da.SaveData(sql, new
    {
        model.FirstName,
        model.LastName,
        model.Gender,
        model.TeleNr,
        model.Place,
        model.FieldOfWork,
        model.Education,
        model.Experience,
        model.DesiredFunction,
        model.Motivation,
        model.Location,
        model.Comments
    }, conn);
    return Results.Created("/api/users/jobagency", null);
});

app.Run();
