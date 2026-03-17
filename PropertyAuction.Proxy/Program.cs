var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpClient();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

var app = builder.Build();
app.UseCors();

app.MapPost("/chat", async (ChatRequest request, IHttpClientFactory factory) =>
{
    var apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY");
    
    if (string.IsNullOrEmpty(apiKey))
        return Results.Problem("API key not configured");

    var http = factory.CreateClient();
    http.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

    var response = await http.PostAsJsonAsync(
        "https://api.openai.com/v1/chat/completions",
        request.Body
    );

    var content = await response.Content.ReadAsStringAsync();
    return Results.Content(content, "application/json");
});

app.Run();

record ChatRequest(object Body);