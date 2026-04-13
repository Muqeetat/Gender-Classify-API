using GenderClassifyAPI.Models;
using GenderClassifyAPI.Services;
using Microsoft.AspNetCore.Http.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<JsonOptions>(options =>
{
    options.SerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.SnakeCaseLower;
});

// --- Services Configuration ---
builder.Services.AddCors(options => {
    options.AddPolicy("AllowAll", policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

builder.Services.AddHttpClient("GenderizeClient", client => {
    client.BaseAddress = new Uri("https://api.genderize.io/");
});

// Register our service
builder.Services.AddScoped<IGenderService, GenderService>();

// This ensures your app binds to 0.0.0.0 and the PORT provided by Railway
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
builder.WebHost.UseUrls($"http://0.0.0.0:{port}");

var app = builder.Build();

app.UseRouting();
app.UseCors("AllowAll");

// --- Route Handling ---
app.MapGet("/api/classify", async (HttpContext context, string? name, IGenderService genderService) =>
{
    context.Response.Headers.Append("Access-Control-Allow-Origin", "*");

    // 1. Validation
    if (string.IsNullOrWhiteSpace(name))
        return Results.Json(new ApiResponse("error", Message: "Missing or empty name"), statusCode: 400);

    if (double.TryParse(name, out _))
        return Results.Json(new ApiResponse("error", Message: "Unprocessable Entity: name must be a string"), statusCode: 422);

    // 2. Delegate to Service
    var (success, result, statusCode) = await genderService.ClassifyNameAsync(name);

    // 3. Return formatted response
    return success
        ? Results.Json(new { status = "success", data = result })
        : Results.Json(result, statusCode: statusCode);
});

app.Run();