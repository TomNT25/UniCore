using System.Reflection;
using Microsoft.OpenApi;
using StimulationEmailProvider.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<InMemoryEmailStore>();
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Stimulated Google Provider API",
        Version = "v1",
        Description = "Mock Google Identity Provider service for issuing and verifying simulated Google OAuth 2.0 ID tokens for local development and testing."
    });

    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFilename);
    if (File.Exists(xmlPath))
    {
        options.IncludeXmlComments(xmlPath);
    }
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// Enable Swagger and Swagger UI
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Stimulated Google Provider API v1");
    options.RoutePrefix = "swagger";
});

app.MapGet("/", () => Results.Redirect("/email/messages?page=1&pageSize=20"))
    .ExcludeFromDescription();

app.MapGet("/health", () => Results.Ok(new { status = "ok", service = "StimulationEmailProvider" }));

app.MapControllers();

app.Run();
