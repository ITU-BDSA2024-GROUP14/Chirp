using Chirp.CSVDBService;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var database = CheepDatabase.Instance;

if (app.Environment.IsDevelopment())
{
    database.ChangeCsvPath("../../data/chirp_cli_dev_db.csv");
}
    
app.MapPost("/cheep", (Cheep cheep) => database.Store(cheep));
app.MapGet("/cheeps", (int? limit = null) => database.Read(limit));

app.Run();