using Chirp.CSVDBService;
using Microsoft.AspNetCore.Http.HttpResults;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var database = CheepDatabase.Instance;

if (app.Environment.IsDevelopment())
{
    database.ChangeCsvPath("../../data/chirp_cli_dev_db.csv");
}
    
app.MapPost("/cheep", Results<BadRequest<string>, Ok> (Cheep cheep) =>
{
    if (string.IsNullOrWhiteSpace(cheep.Author))
    {
        return TypedResults.BadRequest("Author is required");
    }

    if (string.IsNullOrWhiteSpace(cheep.Message))
    {
        return TypedResults.BadRequest("Message is required");
    }

    database.Store(cheep);

    return TypedResults.Ok();
});
app.MapGet("/cheeps", (int? limit = null) => database.Read(limit));

app.Run();