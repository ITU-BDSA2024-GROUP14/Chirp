using Chirp.CSVDBService;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var database = CheepDatabase.Instance;

app.MapPost("/cheep", (Cheep cheep) => database.Store(cheep));
app.MapGet("/cheeps", () => database.Read());

app.Run();