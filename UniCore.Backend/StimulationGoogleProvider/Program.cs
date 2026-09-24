using Microsoft.OpenApi;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
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

// Enable Swagger and Swagger UI
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Stimulated Google Provider API v1");
    options.RoutePrefix = "swagger";
});

app.UseHttpsRedirection();

// Redirect root to swagger UI
app.MapGet("/", () => Results.Redirect("/swagger"));

app.MapControllers();

app.Run();
