var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapPost("/cheep", () => "Hello World!");
app.MapGet("/cheeps", () => "Hello World!");


app.Run();