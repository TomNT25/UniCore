using StimulationEmailProvider.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<InMemoryEmailStore>();
builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapGet("/", () => Results.Redirect("/email/messages?page=1&pageSize=20"))
    .ExcludeFromDescription();

app.MapGet("/health", () => Results.Ok(new { status = "ok", service = "StimulationEmailProvider" }));

app.MapControllers();

app.Run();
